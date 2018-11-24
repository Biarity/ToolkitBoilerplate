# Git Setup

```
git clone https://github.com/Biarity/ToolkitBoilerplate.git;
cd ToolkitBoilerplate;
git remote rm origin;
git remote add boilerplate https://github.com/Biarity/ToolkitBoilerplate.git;
git config remote.boilerplate.pushurl "DONT PUSH TO BOILERPLATE";
git remote add origin $PROJECT_ORIGIN
```

* Push local `master` to origin remote (project-specific remote): `git push origin master`
* Pull local `master` to boilerplate remote: `git pull boilerplate master`

# Renaming

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
