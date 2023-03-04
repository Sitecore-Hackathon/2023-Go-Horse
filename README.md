# CDP Personalize Connector for Sitecore XM Cloud

## Team
⟹ **Go Horse**
- João Amancio Neto
- José Neto
- Rodrigo Peplau

## Category
⟹ Best Enhancement  to XM Cloud

## Description
Introducing our innovative solution for the Sitecore Hackathon 2023 - a CDP Personalize Connector for Sitecore XM Cloud. This feature enables the separation of responsibilities by keeping content in the CMS while housing personalization logic in the CDP.

With our solution, users can seamlessly manage and update content in Sitecore XM Cloud while leveraging the advanced personalization capabilities of the CDP. This streamlined approach allows for a more efficient workflow and ultimately delivers a superior user experience.

By maintaining the separation of responsibilities, our CDP Personalize Connector ensures that content and personalization are handled by the appropriate systems, reducing the risk of errors and simplifying the content management process.

## Video link
⟹ Provide a video highlighing your Hackathon module submission and provide a link to the video. You can use any video hosting, file share or even upload the video to this repository. _Just remember to update the link below_

⟹ [Replace this Video link](#video-link)

## Pre-requisites and Dependencies

Before proceeding, please make sure you have the following:

- (Optional) A CDP account. If you don't have one, don't worry, our demo already has a default account.
- Docker for Windows installed and properly configured. You can find installation instructions [ here ](https://docs.docker.com/desktop/install/windows-install/).

## Installation instructions

1. Open `PersonalizeConnect.sln` and publish it without building/rebuilding the solution.

> Note: It's important to avoid building or rebuilding the solution as it may break the CM container. However, if you accidently do so, make sure to run a Clean solution before publishing it.

2. Open an ADMIN terminal and execute the following command

    ```ps1
    .\init.ps1 -InitEnv -LicenseXmlPath "C:\path\to\license.xml" -AdminPassword "DesiredAdminPassword"
    ```
> Make sure to replace "C:\path\to\license.xml" with the actual file path of your license.xml file and  "DesiredAdminPassword" with a desired password for the admin account.

3. Now, execute the following command:

    ```ps1
    .\up.ps1
    ```

## Usage instructions
⟹ Provide documentation about your module, how do the users use your module, where are things located, what do the icons mean, are there any secret shortcuts etc.

Include screenshots where necessary. You can add images to the `./images` folder and then link to them from your documentation:

![Hackathon Logo](docs/images/hackathon.png?raw=true "Hackathon Logo")

You can embed images of different formats too:

![Deal With It](docs/images/deal-with-it.gif?raw=true "Deal With It")

And you can embed external images too:

![Random](https://thiscatdoesnotexist.com/)

## Comments
If you'd like to make additional comments that is important for your module entry.
