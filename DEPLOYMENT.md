# MealTimes — Deployment Guide

Backend → **Render** (Docker) · Database → **Neon** (PostgreSQL) · Frontend → **Vercel**

> Neon is used for the database instead of Render's free Postgres because Render's free
> database is deleted after ~30 days, while Neon's free tier is permanent (no card required).

The code is already prepared for deployment:
- EF Core switched from SQL Server to **PostgreSQL** (Npgsql).
- Connection string, allowed CORS origins, and listening port all read from **environment variables**.
- Migrations are applied **automatically on startup** (`db.Database.Migrate()`), so you don't run any manual DB commands.
- A fresh PostgreSQL migration (`InitialPostgres`) has been generated.

---

## 0. Before you start — rotate the Stripe secret key ⚠️

Your **Stripe secret key** is committed in `appsettings.json` and pushed to GitHub, so treat it as
compromised. Go to the [Stripe Dashboard → Developers → API keys](https://dashboard.stripe.com/apikeys),
**roll** the secret key, and use the new value as an environment variable on Render (step 2).
Do **not** commit the new key. (The *publishable* key `pk_...` is safe to be public.)

---

## 1. Create the database (Neon PostgreSQL)

1. Sign up / log in at https://neon.tech (Sign in with GitHub).
2. **Create project** → name `mealtimes` → pick a region (e.g. `us-east-1`).
3. On the project page open **Connect** / **Connection Details** and copy the **full connection
   string**. It looks like:
   ```
   postgresql://USER:PASSWORD@ep-xxxx-pooler.c-N.REGION.aws.neon.tech/neondb?sslmode=require&channel_binding=require
   ```
   - Use the **Pooled connection** (host contains `-pooler`) — best for a web app with many
     short-lived connections.
   - **Copy it exactly**, including the `.c-N.` part of the host and all query params. That `.c-N.`
     segment is Neon's routing host (sent via SNI); removing it causes `password authentication failed`.
4. This whole string is your `DATABASE_URL` (used in step 2). The app already knows how to parse
   Neon's URL format and connect over SSL.

> The schema for this project has **already been applied** to the Neon database during setup,
> so all tables exist. On deploy the app re-checks migrations and simply starts (no-op).
> If you create a *new* Neon DB later, the app auto-applies migrations on first boot.

---

## 2. Deploy the backend (Render Web Service, Docker)

1. **New +** → **Web Service** → connect the **`MealTimes-Backend`** GitHub repo.
2. Settings:
   - **Runtime / Language**: Render auto-detects the `Dockerfile` → choose **Docker**.
   - **Region**: pick one close to your Neon region (e.g. **US East (Ohio/Virginia)** for Neon `us-east-1`) to keep DB latency low.
   - **Branch**: `master`  *(the backend repo's default branch is `master`, not `main`)*
   - **Plan**: **Free**
3. **Environment variables** (Advanced → Add Environment Variable):

   | Key | Value |
   |-----|-------|
   | `DATABASE_URL` | *(paste the full Neon connection string from step 1, exactly as-is)* |
   | `AllowedOrigins` | `https://YOUR-APP.vercel.app` *(fill in after step 3; can edit later)* |
   | `JwtSettings__Key` | a long random string (≥ 32 chars) |
   | `JwtSettings__Issuer` | `MealTimesApi` |
   | `JwtSettings__Audience` | `MealTimesClient` |
   | `JwtSettings__ExpiryMinutes` | `60` |
   | `Stripe__SecretKey` | your **rotated** `sk_...` key |
   | `Stripe__PublicKey` | your `pk_...` key |
   | `EmailSettings__SmtpHost` | `smtp.gmail.com` |
   | `EmailSettings__SmtpPort` | `587` |
   | `EmailSettings__FromEmail` | your sender email |
   | `EmailSettings__FromPassword` | your Gmail **app password** |
   | `EmailSettings__FromName` | `MealTimes Support` |
   | `AppSettings__BaseUrl` | `https://YOUR-APP.vercel.app` |

   > Note the **double underscore** `__` — that's how .NET maps env vars to nested config sections.
   > Do **not** set `PORT`; Render sets it automatically and the app reads it.

4. **Create Web Service**. First build takes a few minutes (Docker build + publish).
5. When live, your API base is `https://mealtimes-backend-xxxx.onrender.com`.
   Test it: open `https://mealtimes-backend-xxxx.onrender.com/swagger` — you should see the API docs.
   On startup the app auto-creates all tables in the Postgres DB.

> **Free-tier note:** Render free web services **sleep after 15 min idle**; the next request takes
> ~30–50s to wake. Fine for a demo — just hit it once before you present.

---

## 3. Deploy the frontend (Vercel)

1. Sign up / log in at https://vercel.com (Sign in with GitHub).
2. **Add New… → Project** → import the **`MealTimes-Frontend`** repo.
3. Vercel auto-detects **Vite**. Leave build command `npm run build` and output dir `dist`.
4. **Environment Variables** — add:

   | Key | Value |
   |-----|-------|
   | `VITE_API_URL` | `https://mealtimes-backend-xxxx.onrender.com/api` |
   | `VITE_STRIPE_PUBLISHABLE_KEY` | your `pk_...` key |
   | `VITE_APP_TITLE` | `Corporate Meal Management` |

   > **Important:** `VITE_API_URL` must end with **`/api`** (the controllers route under `api/...`).

5. **Deploy**. You'll get a URL like `https://your-app.vercel.app`.

---

## 4. Connect the two (CORS)

1. Copy your Vercel URL.
2. On Render → backend service → **Environment** → set
   `AllowedOrigins` = `https://your-app.vercel.app` and `AppSettings__BaseUrl` = same.
   - To allow Vercel preview URLs too, comma-separate them:
     `https://your-app.vercel.app,https://your-app-git-main-you.vercel.app`
3. Save → Render redeploys automatically. Done.

---

## Local development after these changes

The app now uses PostgreSQL locally too. Either install Postgres locally, or just keep developing
against a Postgres connection string:

```powershell
# point local runs at any Postgres (or set DATABASE_URL)
$env:DATABASE_URL = "Host=localhost;Port=5432;Database=MealTimes;Username=postgres;Password=postgres"
dotnet run --project MealTimes.Controller
```

Frontend: `.env` already has `VITE_API_URL=https://localhost:7000/api` for local use.

### Regenerating migrations later
```powershell
$env:DOTNET_ROLL_FORWARD = "LatestMajor"   # only needed if you don't have the .NET 8 runtime
dotnet ef migrations add <Name> `
  --project MealTimes.Repository `
  --startup-project MealTimes.Controller
```
