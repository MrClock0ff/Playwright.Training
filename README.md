# Prerequisites
- Visual Studio Code (VS Code)
- Node.js (node) with Node Package Manager (npm)
- Docker
- Postman (optional)

# Container Setup Guide
1. Open VS Code and then open project folder **"Playwright.Training.Api"**
2. Install **C# Dev Kit** Extension for VS Code
3. Once **C# Dev Kit** extension is installed it may show warning **".NET SDK not found"**. Click on **"Get SDK"** button. **"Install New .NET SDK"** tab will appear in VS Code. Select **"Others"** and then select **".NET 8.0"**. Hit **"Install .NET SDK"** button. Follow the prompts to complete .NET SDK installation.
4. Restart VS Code upon successful .NET SDK installation.
5. Select **"Explorer"** side tab in VS Code and expand **"Solution Explorer"** section. It should contain project solution.
6. Open terminal window (VS Code terminal window is perfect for this task) and ensure current directory is set to project directory.
7. Execute command `docker compose up -d` (to restart container simply execute command `docker compose down`) inside the terminal window to create project Docker container.
8. Check that Docker container is all set and running. Container group name should be **"playwrighttrainingapi"**.
9. At this point Keycloak Playwright realm users with their permissions and default Playwright API user will be created.

All good to go!!!

# Postman Setup Guide (Optional)
1. Open Postman and click **"Import"** button inside side panel on the left hand side.
2. Collection file is located inside root project folder and is called `postman_collection.json`.
3. Click **"Variables"** button inside side panel on the right hand side.
4. Inside the same side panel locate **"[Playwright Training]"** group of variables.
5. Set `playwright_traning_api_url` value as `http://localhost:8081`.
6. Set `keycloak_url` value as `http://localhost:8180`.

All good to go!!! Collection folder can be used to get access token under **"Authorization"** tab.

Collection contains whole API setup and can be used for endpoint execution.

# "keycloak" Container
- Admin console URL is [http://localhost:8180](http://localhost:8180).
- Admin console username is `keycloak` and password is `mysecretpassword`.
- Playwright realm first user username is `user1` and password is `12345678`.
- Playwright realm second user username is `user2` and password is `87654321`.

# "playwright.training.api" Container
- Public API URL [http://localhost:8081/swagger/index.html](http://localhost:8081/swagger/index.html).
- Default user UID is `5c33e35c-606e-4d8f-91ec-1eba45094043`.

# "postgres" Container