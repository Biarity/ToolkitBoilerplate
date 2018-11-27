# Setup

## Git

Replace `$YOUR_PROJECT_ORIGIN` with your new project's origin

```bash
git clone https://github.com/Biarity/ToolkitBoilerplate.git
cd ToolkitBoilerplate
git remote rm origin
git remote add boilerplate https://github.com/Biarity/ToolkitBoilerplate.git
git config remote.boilerplate.pushurl "(do not push to boilerplate)"
git remote add origin $YOUR_PROJECT_ORIGIN
```

* Push local `master` to `origin` remote (project-specific remote): `git push origin master`
* Pull local `master` to `boilerplate` remote: `git pull boilerplate master`

## Renaming

* In VisualStudio
    * Rename project
    * Rename solution
    * Rename namespace
* Open up `appsettings.json` and `appsettings.Development.json`
	* Replace stuff in square brackets
* Open up `/ClientApp/project.json`
    * Rename `name` key
* Open up `/ClientApp/public/manifest.json`
    * Rename `name` and `short_name` keys
* Open up `STATR_DEV.bat`
	* Rename cd line to project name

## Start dev servers

Open `START_DEV.bat`.

Or...in the project directory,
* `dotnet watch run` to start backend on port 5000
* `npm run serve --prefix ClientApp` to start frontend on port 8080
* Note in dev backend reverse proxies frontend so no need to got o port 8080

## Next steps

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