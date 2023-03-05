# Personalize Connector for XM Cloud

## Team
⟹ **Go Horse**
- João Amancio Neto
- José Neto
- Rodrigo Peplau

![Go Horse](https://github.com/Sitecore-Hackathon/2020-Team-Go-Horse/raw/master/documentation/images/gohorse.jpg?raw=true)

## Category
⟹ Best Enhancement  to XM Cloud

## Description
Introducing our innovative solution for the Sitecore Hackathon 2023 - a CDP & Personalize Connector for Sitecore XM Cloud. This feature enables the separation of responsibilities by keeping content in the CMS while housing personalization logic in the CDP.

With our solution, users can seamlessly manage and update content in Sitecore XM Cloud while leveraging the advanced personalization capabilities of the CDP. This streamlined approach allows for a more efficient workflow and ultimately delivers a superior user experience.

By maintaining the separation of responsibilities, our Connector ensures that content and personalization are handled by the appropriate systems, reducing the risk of errors and simplifying the content management process.

## Video link

⟹ [Video: Personalize Connector for XM Cloud](https://youtu.be/mPGGNcIrB_s)

## Pre-requisites and Dependencies

Before proceeding, please make sure you have the following:

- (Optional) A CDP account. If you don't have one, don't worry, our demo already has a default account.
- Docker for Windows installed and properly configured. You can find installation instructions [ here ](https://docs.docker.com/desktop/install/windows-install/).

## Installation instructions

1. Clone this repository

2. Open an ADMIN terminal and execute the following command

    ```ps1
    .\init.ps1 -InitEnv -LicenseXmlPath "C:\path\to\license.xml" -AdminPassword "DesiredAdminPassword"
    ```
> Make sure to replace "C:\path\to\license.xml" with the actual file path of your license.xml file and  "DesiredAdminPassword" with a desired password for the admin account.

3. Now, execute the following command:

    ```ps1
    .\up.ps1
    ```

4. Open `PersonalizeConnect.sln` and publish it without building/rebuilding the solution.

> Note: It's important to avoid building or rebuilding the solution as it may break the CM container. However, if you accidently do so, make sure to run a Clean solution before publishing it.


## Usage instructions

### Confirming that the module is working properly

To confirm that the module is working properly, access the following URL: https://xmcloud.local

* Immediately after the page loads, you should see the default datasource.

![Default Datasource](docs/images/01-DefaultDatasource.png?raw=true "")

* After Personalize finishes processing, the default datasource should be replaced with variation 1 ...

![Variation 1](docs/images/02-VAR1.png?raw=true)

... or variation 2

![Variation 2](docs/images/03-VAR2.png?raw=true)

With this, you'll know that Personalize is doing its magic!

### Configuring personalization on XM Cloud

Content Editors can configure personalization on XM Cloud the same way they do with any normal personalization. The module with a custom condition which is responsible for communicating the proper datasource to CDP.

![Personalize Condition](docs/images/04-Condition.png?raw=true)

The Web Experience ID can be taken from the personalize experience URL as shown below:

![Experience ID](docs/images/06-ExperienceId.png?raw=true)

The experience value is an arbitrary value that you can determine as part of your web experience logic in Personalize. For instance, this value can be an index, ages, geo region, or any other value that makes sense to your business.  

This is how we configured this demo in the home page:

![OOTB Personalization](docs/images/05-PersonalizationSetup.png?raw=true)
<br /><br />
### Hooking up your component with Personalize

In order to have Personalize automatically binding the datasource to your component, you have to add attributes to the markup as seen below:

![Component Markup](docs/images/07-ComponentMarkup.png?raw=true)

* `cdp-container`: the guid of the datasource item, representing the component container.
* `cdp-field`: the field name to be rendered in that HTML element.

### Configuring your Personalize Web Experience

It's very easy to retrieve and use the datasource item from XM-Cloud inside Personalize using our helper methods.

```javascript
(function() {
  // Randomly sets experienceValue to 1 or 2
  const experienceValue = Math.floor(Math.random() * 2) + 1;

  // Get datasource from CMS
  const datasource = GetDatasource(debugFlow.definition.ref, experienceValue);

  // Automatically render fields
  PopulateBlock(datasource);
})();
```

If the `PopulateBlock` method is not enough to render all your fields, you can always use the datasource object directly with JavaScript to implement your complex business logic.

