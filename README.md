# Rename

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

# Setup

* Make boilerplate repo pull-only
	* `git remote add boilerplate https://BOILERPLATE_REPO_LOCATION.git`
	* `git config remote.boilerplate.pushurl "DONT PUSH TO BOILERPLATE"`
	* Then to get updates from the boilerplate `git pull boilerplate`