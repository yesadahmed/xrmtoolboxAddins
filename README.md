
# JsonToCsharp
A xrmtoolbox plugin for Dynamic36 webapi to convert the json to CSharp models. You can also use this tool to convert any json to c# models.
This tool requires only **OAuth or Certificate** types connection to get crm entities as it connects to WebAPI.
<br/>For how to connect and working examples please see below.

## How to Connect in xrmtoolbox (connection Types)
Once you have the xrmtoolbox you need to install this plugin form Tool Library as shown below.
![xrmtoolbox connections](https://github.com/yesadahmed/xrmtoolboxAddins/blob/main/JsonToCSharp/images/library.png)

Once the installion is done, you will see this plugin as follows:
![xrmtoolbox connections](https://github.com/yesadahmed/xrmtoolboxAddins/blob/main/JsonToCSharp/images/plugin.PNG)

Since this plugin connects to CE webapi so by default it requires **OAuth or Certifcate** type connections in xrmtoolbox.
<br/>For example regarding available OAuth connections in xrmtools are mentioned below:

![xrmtoolbox connections](https://github.com/yesadahmed/xrmtoolboxAddins/blob/main/JsonToCSharp/images/Conn1.png)

Some examples are as follows.

![xrmtoolbox connections](https://github.com/yesadahmed/xrmtoolboxAddins/blob/main/JsonToCSharp/images/sdkcontrol.png)

![xrmtoolbox connections](https://github.com/yesadahmed/xrmtoolboxAddins/blob/main/JsonToCSharp/images/conneciont.PNG)
 AuthType=OAuth;Username=jsmith@contoso.onmicrosoft.com; Password=passcode;
Url=https://contoso:8080/Test;AppId=<GUID>;RedirectUri=app://<GUID>; LoginPrompt=Never

## Examples
Once you are connected through OAuth connection, by default the plugin will load all the mostly used entities as list.
By default it will load the accounts entity and display only one random record, so the json is availble for conversion
or customization or properties changes.<br/>Also you can entirely replace the crm json and paste your own
valid json for relevant csharp classes. Please see the screen shots below.

## Crm Entities Loaded
![entities loaded](https://github.com/yesadahmed/xrmtoolboxAddins/blob/main/JsonToCSharp/images/entities_loaded.png)

## Crm Entities Json
![entities json](https://github.com/yesadahmed/xrmtoolboxAddins/blob/main/JsonToCSharp/images/convert_crm_json.png)

## Custom Json
![entities json](https://github.com/yesadahmed/xrmtoolboxAddins/blob/main/JsonToCSharp/images/custom%20json.png)


<br/><br/>
For more help about CE connection strings. [here](https://docs.microsoft.com/en-us/previous-versions/dynamicscrm-2016/developers-guide/mt608573(v=crm.8)?redirectedfrom=MSDN) <br/>
See more info about CE WebAPI authentication methods. [here](https://docs.microsoft.com/en-us/dynamics365/customerengagement/on-premises/developer/webapi/authenticate-web-api). <br/>
For more help you can raise an issuse [here](https://github.com/yesadahmed/xrmtoolboxAddins/issues)<br/>

Just an reminder the null values in json crossponds to object in c# class, becuase the parser expect some valid cli types instead we 
have null in value and hence currently parser is not able to determine the valid type and it puts object type.

## Next Releases Features (1.0.0.2) :+1:
1. Will add the possiblity to select entities and their attributes to build json. (select or search)
2. Formatted json display in box.

Feel free let me know about new features you want and any improvents you thought.
I will try to address as soon as possible.<br/>
you can riase new issues/fearures [here](https://github.com/yesadahmed/xrmtoolboxAddins/issues).

**Adnan Samuel**

