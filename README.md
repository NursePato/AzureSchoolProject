### Setup
- Cloning the repo to my local machine
- Setting up an own repository in my github
- Checking if the code works locally before pushing -> I was missing .net runtime, so I installed  
the required software, and later on tried again with `dotnet run`, works now and I pushed the application to my repo.

### Creating Azure App Service
1. Creating and setting up a Static Web App through the Azure portal.
2. Modifying the YAML file to contain the correct build and path
3. Fixing the Azure web app Stack & Version under Settings -> Configuration -> General Settings

4. **Fighting with Error 409** ```Package deployment using OneDeploy initiated.
Error: Failed to deploy web package to App Service.
Error: Deployment Failed, Error: Failed to deploy web package using OneDeploy to App Service.
Conflict (CODE: 409)
App Service Application URL: https://azureschoolproject.azurewebsites.net```

5. *Modifying YAML* Through the line of code that teacher Sebastian provided, the build and deploy now works, CI/CD is setup through github actions.

also this: ```🔍 Root Cause
The issue you're running into is because az webapp deployment source config-zip is asynchronous — meaning it starts the deployment and then just waits for a bit to see if it finishes. If it doesn’t get confirmation in time, it throws a timeout even though deployment might still be going through fine, your app still deploys correctly, so this is just a false negative.```
- Since the build sometimes *fails* even though it builds and deploys it correctly

### Activating Application Insights
- Made sure to activate Application Insight at the creation stage of the app service.
- Inside my Application on the portal, I went down to Monitoring -> Application Insights and enabled Collection level
- Now I'm able to check logs through Kusto (KQL), and get an overwiev of the application and its metrics trough the overview tab. Here I can for exmaple view performance, availability and failures. But I can also go into Investigate -> Live metrics and get a live overview of my application.

### Adding fundamental security measures
- Through IAM -> Add role assignment -> Reader role, apply it to fredrik.dahl@consid.se
- Signing up to https://www.noip.com/ -> creating free domain -> trying to verify it to my domain, but the free version doesn't allow DNS record customization, and I can therefore not use this domain.
- Since I am using Azure App Service, I get SSL automatically activated, and having HTTPS only active, makes sure the communication is encrypted.

### Automatic scaling
- With my current plan Basic (B1) I am unable to access the feature automtic scaling. If I would like to add automatic scaling, I would have to at least scale up to Premium v3 P0V3. For testing purposes I will.
- Go to App services -> click App service plan -> Scale up -> choose the required plan (Premium v3 P0V3)
- Go to Scale out -> and check either automatic scaling or Rules scaling.
- To later track this, you can check Monitor -> Activity log and through Application Inslights, although Application Insights will show spikes in activity and where scaling might have happened.

### Creating Azure Storage Account
- Storage Accounts -> Create -> picking standard and low cost options, local redundancy -> keep storage private for security reasons(which is baseline).
- Enter the newly created storage account -> under Data storage go to Containers and made a container *game-img-covers* -> Upload imgs I want to display on application. 
- Tie the pictures to the correct json object through SAS url, and made sure to set expiry a month forward.

### Creating a Key Vault
- Creating a Key Vault to store sensitive data, my goal is to hold an api-key and fetch it through the code.
- Search for Key Vault in Azure portal -> Create -> Name it -> Leaving Enable public access checked as I don't want to setup IAM/manage ip-adresses for this project.
- First hurdle, RBAC, Im not allowed to read secrets from Azure Key Vault with the student account we are on, so I will mock the key vault, just to show how I would have done it.
#### Update after new information from classmates
- Apparently we can give us access through IAM by pressing Add -> Add Role assignment -> search for Key vault administrator, select it -> Go to Members and search and add your account.
- Through Key Vault -> secrets -> Generate/import -> Name it & add your secret value, create.

#### Mocking api key
- adding this code to appsettings.json: "FakeApiKey": "1234-5678-FAKE-KEY",
- Injecting into GameService.cs, building in Program.cs
- See code under Program.cs & GameService.cs for examples

### Implement a CI/CD-Pipeline in Azure DevOps
- Had to activate *Allow public projects* to be able to create a public project.
- Import my github repo through the Repos -> Import Repos
- Go to Pipelines -> Select YAML -> Authorize connection.
- Setting up yml file by taking my github actions yml and converting it to DevOps syntax, save and run. Eventually when the YML file works, you should hit this error code
```
Errors 
1
No hosted parallelism has been purchased or granted. To request a free parallelism grant, please fill out the following form https://aka.ms/azpipelines-parallelism-request
20250410.8
```
- Since we are limited to 1 parallel action with our Azure student account, and our Github actions is consuming that action.

#### Update after requesting lifting parallelism through the available Microsoft form
- I am now able to deploy through my pipeline in Azure DevOps
