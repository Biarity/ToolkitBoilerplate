# Setup

1. Replace `$YOUR_PROJECT_ORIGIN` below with your new project's origin and run

```bash
git clone https://github.com/Biarity/ToolkitBoilerplate.git . 
&& git remote rm origin 
&& git remote add boilerplate https://github.com/Biarity/ToolkitBoilerplate.git 
&& git config remote.boilerplate.pushurl "(do not push to boilerplate)" 
&& git remote add origin $YOUR_PROJECT_ORIGIN 
&& cd ToolkitBoilerplate 
&& dotnet restore 
&& dotnet ef migrations add Init 
&& cd ClientApp 
&& npm install 
``` 

2. Update `appsettings.json` and `appsettings.Development.json` in /ToolkitBoilerplate

3. Run `dotnet ef database update` in /ToolkitBoilerplate

* To push local `master` to `origin` remote (project-specific remote): `git push origin master`
* To pull local `master` to `boilerplate` remote: `git pull boilerplate master`

## Renaming

* In VisualStudio
    * Rename project
    * Rename solution
    * Rename namespace
* Open up `/ClientApp/project.json`
    * Rename `name` key
* Open up `/ClientApp/public/manifest.json`
    * Rename `name` and `short_name` keys
* Open up `STATR_DEV.bat`
	* Rename cd line to project name

## Dev Servers

```bash
cd ToolkitBoilerplate
start cmd /k npm run serve --prefix ClientApp
start cmd /k dotnet watch run
```

Or...Open `START_DEV.bat`.

That is, in the project directory,
* `dotnet watch run` to start backend on port 5000
* `npm run serve --prefix ClientApp` to start frontend on port 8080
* Note in dev backend reverse proxies frontend so no need to got to port 8080

# Next steps

* Add entities (should be subclasses of `ApplicationEntity`)
* Create migrations & update database (`Add-Migration Init; Update-Database`)
* Create controllers for each entity (should be subclasses of `ApplicationController`, can use `/swagger` to debug)
* Start on frontend

# Deploy

* Build frontend
* Build backend
* Docker? 
* TODO

# TODO

- [ ] Vue boilerplate for login, flags, alerts, comments, notifications
- [ ] Controllers and entities for boilerplate for comments, reactions, notifications, flagging
- [ ] `ApplicationHub` boilerplate
- [ ] Deployment plan and instructions
