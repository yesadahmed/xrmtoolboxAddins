using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using XrmToolBox.Extensibility;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk;
using McTools.Xrm.Connection;
using XrmToolBox.Extensibility.Interfaces;
using Microsoft.Xrm.Tooling.Connector;
using Microsoft.Xrm.Sdk.Client;
using System.Net.Http;

using System.IO;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;
using System.Collections.Specialized;

namespace JsonToCSharp
{
	public partial class MyPluginControl : PluginControlBase
	{

		private Settings mySettings;


		public MyPluginControl()
		{
			InitializeComponent();
		}
		/*
             private void MyPluginControl_Load(object sender, EventArgs e)
             {
                 ShowInfoNotification("This is a notification that can lead to XrmToolBox repository", new Uri("https://github.com/MscrmTools/XrmToolBox"));

                 // Loads or creates the settings for the plugin
                 if (!SettingsManager.Instance.TryLoad(GetType(), out mySettings))
                 {
                     mySettings = new Settings();

                     LogWarning("Settings not found => a new settings file has been created!");
                 }
                 else
                 {
                     LogInfo("Settings found and loaded");

                 }


             }

             private void tsbClose_Click(object sender, EventArgs e)
             {
                 CloseTool();
             }

             private void tsbSample_Click(object sender, EventArgs e)
             {
                 // The ExecuteMethod method handles connecting to an
                 // organization if XrmToolBox is not yet connected
                 //ExecuteMethod(GetAllEntities);
             }*/



		/// <summary>
		/// This event occurs when the plugin is closed
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		/*  private void MyPluginControl_OnCloseTool(object sender, EventArgs e)
		  {
			  // Before leaving, save the settings
			  SettingsManager.Instance.Save(GetType(), mySettings);
		  }*/

		/// <summary>
		/// This event occurs when the connection has been updated in XrmToolBox
		/// </summary>
		public override void UpdateConnection(IOrganizationService newService, ConnectionDetail detail, string actionName, object parameter)
		{
			base.UpdateConnection(newService, detail, actionName, parameter);

			if (mySettings != null && detail != null)
			{
				mySettings.LastUsedOrganizationWebappUrl = detail.WebApplicationUrl;
				LogInfo("Connection has changed to: {0}", detail.WebApplicationUrl);
			}
			ExecuteMethod(GetAllEntities);
		}

		private void lblClose_Click(object sender, EventArgs e)
		{
			CloseTool();
		}

		private void label2_Click(object sender, EventArgs e)
		{

		}


		#region HelperFunctions

		private bool IsValidJson(string input)
		{
			if (string.IsNullOrWhiteSpace(input)) { return false; }
			input = input.Trim();
			return input.StartsWith("{") && input.EndsWith("}")
				   || input.StartsWith("[") && input.EndsWith("]");
		}


		private void GetAllEntities()
		{

			//WorkAsync(new WorkAsyncInfo
			//{

			//	Message = "Please wait loading crm entities ....",
			//	Work = (worker, args) =>
			//	{
			//		try
			//		{
			//			args.Result = ConnectionDetail.ServiceClient.ExecuteCrmWebRequest(HttpMethod.Get,
			//			"EntityDefinitions?$select=LogicalName,DisplayName,EntitySetName", null, null, "application/json");
			//		}
			//		catch (Exception ex)
			//		{
						
			//		}

			//	},
			//	PostWorkCallBack = (args) =>
			//	{
			//		if (args.Error != null)
			//		{
			//			MessageBox.Show(args.Error.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			//		}

			//		try
			//		{
			//			var result = args.Result as HttpResponseMessage;

			//			if (result != null && result.IsSuccessStatusCode)
			//			{
			//				HideNotification();
			//				var status = result.StatusCode;
			//				var json = result.Content.ReadAsStringAsync().Result;
			//				JavaScriptSerializer ser = new JavaScriptSerializer();

			//				var allEntities = ser.Deserialize<AllEntities>(json);

			//				if (allEntities != null && allEntities.value != null && allEntities.value.Count > 0)
			//				{
			//					List<EntityModel> entitiesToInclude = new List<EntityModel>();
			//					CommonHelper commonHelper = new CommonHelper();
			//					foreach (var item in allEntities.value)
			//					{
			//						if (commonHelper.skipStartsWith.Any(f => item.LogicalName.StartsWith(f)))
			//							continue;

			//						var localizedLabel = item.DisplayName.LocalizedLabels.FirstOrDefault();//cjeck fo rinternal system entities
			//						if (localizedLabel != null)
			//						{
			//							if (!commonHelper.excludedList.Contains(localizedLabel.Label))
			//							{
			//								if (!(localizedLabel.Label.StartsWith("msdyn_")))
			//								{
			//									entitiesToInclude.Add(new EntityModel()
			//									{
			//										LogicalName = item.LogicalName,
			//										DisplayName = localizedLabel.Label,
			//										EntitySetName = item.EntitySetName
			//									});
			//								}

			//							}
			//						}

			//					} //foreach
			//					if (entitiesToInclude.Count > 0)
			//					{
			//						entitiesToInclude = entitiesToInclude.OrderBy(ent => ent.LogicalName).ToList();
			//						//bind combo and text
			//						var cmbBindingSource = new BindingSource();
			//						cmbBindingSource.DataSource = entitiesToInclude;

			//						cmbEntities.DataSource = cmbBindingSource.DataSource;

			//						cmbEntities.DisplayMember = "DisplayName";
			//						cmbEntities.ValueMember = "EntitySetName";

			//						// now load account json
			//						LoadNewJson("accounts");
			//					}
			//					//MessageBox.Show($"status {status},  data { data}");
			//				}
			//			}
			//			else
			//			{
			//				VerfiyOAuthConnection();
			//			}

			//		} //try
			//		catch (Exception ex)
			//		{
			//		}

			//	}
			//});
		}

		private void LoadNewJson(string entityName)
		{
			
		}
		#endregion



		private void MyPluginControl_Load_1(object sender, EventArgs e)
		{
			// Loads or creates the settings for the plugin
			if (!SettingsManager.Instance.TryLoad(GetType(), out mySettings))
			{
				mySettings = new Settings();

				LogWarning("Settings not found => a new settings file has been created!");
			}
			else
			{
				LogInfo("Settings found and loaded");

			}
			ExecuteMethod(GetAllEntities);
		}

		private void VerfiyOAuthConnection()
		{
			try
			{
				if (ConnectionDetail != null && ConnectionDetail.ServiceClient != null) //make sure for connection
				{
					if (ConnectionDetail.ServiceClient.ActiveAuthenticationType == Microsoft.Xrm.Tooling.Connector.AuthenticationType.OAuth ||
						   ConnectionDetail.ServiceClient.ActiveAuthenticationType == Microsoft.Xrm.Tooling.Connector.AuthenticationType.Certificate)
					{
						HideNotification();
					}
					else //notfiy and link to which connection will work
					{
						try
						{
							string alert = "This plugin is for Dynamic365 WebAPI so it works only with OAuth or Certificate types connections. Please connect using OAuth connection. Click learn more link. ";
							ShowErrorNotification(alert, new Uri("https://github.com/yesadahmed/xrmtoolboxAddins/blob/main/README.md"));
							alert = "This plugin is for Dynamic365 WebAPI so it works only with OAuth or Certificate types connections. Please connect using OAuth connection. \n\t Follow the link. https://github.com/yesadahmed/xrmtoolboxAddins/blob/main/README.md";
							MessageBox.Show(alert, "OAuth connection required", MessageBoxButtons.OK, MessageBoxIcon.Warning);

						}
						catch (Exception)
						{

						}
					}
				}
				else
				{
					
					ShowErrorNotification("Something wrong with your connection, Try again or make a new OAuth connection.", new Uri("https://github.com/yesadahmed/xrmtoolboxAddins"));
				}
			}
			catch (Exception)
			{


			}
		}

		private void btnConvert_Click(object sender, EventArgs e)
		{
			

		}

		private void cmbEntities_SelectedIndexChanged(object sender, EventArgs e)
		{
		//	var item = cmbEntities.SelectedItem as EntityModel;
		//	LoadNewJson(item.EntitySetName);
		}

		private void MyPluginControl_OnCloseTool_1(object sender, EventArgs e)
		{
			// Before leaving, save the settings
			SettingsManager.Instance.Save(GetType(), mySettings);
		}

		private void btnCopy_Click(object sender, EventArgs e)
		{
		
		}

		private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
		{

		}

		private void MyPluginControl_Resize(object sender, EventArgs e)
		{
			
		}

		private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			try
			{
				// to true.
				linkLabel1.LinkVisited = true;
				//Call the Process.Start method to open the default browser
				//with a URL:
				System.Diagnostics.Process.Start("https://github.com/yesadahmed/xrmtoolboxAddins/blob/main/README.md");
			}
			catch (Exception)
			{
				
			}
		}
	}
}