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
using JsonToCSharp.Models;
using JsonToCSharp.CommonHelpers;
using Xamasoft.JsonClassGenerator.CodeWriters;
using Xamasoft.JsonClassGenerator;
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

			WorkAsync(new WorkAsyncInfo
			{

				Message = "Please wait loading crm entities ....",
				Work = (worker, args) =>
				{
					try
					{
						args.Result = ConnectionDetail.ServiceClient.ExecuteCrmWebRequest(HttpMethod.Get,
						"EntityDefinitions?$select=LogicalName,DisplayName,EntitySetName", null, null, "application/json");
					}
					catch (Exception ex)
					{
						
					}

				},
				PostWorkCallBack = (args) =>
				{
					if (args.Error != null)
					{
						MessageBox.Show(args.Error.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					}

					try
					{
						var result = args.Result as HttpResponseMessage;

						if (result != null && result.IsSuccessStatusCode)
						{
							HideNotification();
							var status = result.StatusCode;
							var json = result.Content.ReadAsStringAsync().Result;
							JavaScriptSerializer ser = new JavaScriptSerializer();

							var allEntities = ser.Deserialize<AllEntities>(json);

							if (allEntities != null && allEntities.value != null && allEntities.value.Count > 0)
							{
								List<EntityModel> entitiesToInclude = new List<EntityModel>();
								CommonHelper commonHelper = new CommonHelper();
								foreach (var item in allEntities.value)
								{
									if (commonHelper.skipStartsWith.Any(f => item.LogicalName.StartsWith(f)))
										continue;

									var localizedLabel = item.DisplayName.LocalizedLabels.FirstOrDefault();//cjeck fo rinternal system entities
									if (localizedLabel != null)
									{
										if (!commonHelper.excludedList.Contains(localizedLabel.Label))
										{
											if (!(localizedLabel.Label.StartsWith("msdyn_")))
											{
												entitiesToInclude.Add(new EntityModel()
												{
													LogicalName = item.LogicalName,
													DisplayName = localizedLabel.Label,
													EntitySetName = item.EntitySetName
												});
											}

										}
									}

								} //foreach
								if (entitiesToInclude.Count > 0)
								{
									entitiesToInclude = entitiesToInclude.OrderBy(ent => ent.LogicalName).ToList();
									//bind combo and text
									var cmbBindingSource = new BindingSource();
									cmbBindingSource.DataSource = entitiesToInclude;

									cmbEntities.DataSource = cmbBindingSource.DataSource;

									cmbEntities.DisplayMember = "DisplayName";
									cmbEntities.ValueMember = "EntitySetName";

									// now load account json
									LoadNewJson("accounts");
								}
								//MessageBox.Show($"status {status},  data { data}");
							}
						}
						else
						{
							VerfiyOAuthConnection();
						}

					} //try
					catch (Exception ex)
					{
					}

				}
			});
		}

		private void LoadNewJson(string entityName)
		{
			WorkAsync(new WorkAsyncInfo
			{

				Message = $"loading {entityName} json ....",
				Work = (worker, args) =>
				{
					try
					{
						args.Result = ConnectionDetail.ServiceClient.ExecuteCrmWebRequest(HttpMethod.Get,
						 $"{entityName}?$top=1", null, null, "application/json");
					}
					catch (Exception)
					{


					}

				},
				PostWorkCallBack = (args) =>
				{
					if (args.Error != null)
					{
						MessageBox.Show(args.Error.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					}
					richTextBoxDest.Text = string.Empty;
					var result = args.Result as HttpResponseMessage;

					if (result != null && result.IsSuccessStatusCode)
					{
						
						var jsonaccountJson = result.Content.ReadAsStringAsync().Result;
						richTextBoxSource.Text = jsonaccountJson;
						var pattern = @"""[A-za-z0-9]*"":";
						List<SelctionPattern> allselections = new List<SelctionPattern>();
						foreach (Match match in Regex.Matches(richTextBoxSource.Text, pattern))
						{
							allselections.Add(new SelctionPattern() { SelectionStart = match.Index, SelectionLength = match.Length, Text = match.Value });

						}
						foreach (var pos in allselections)
						{
							richTextBoxSource.Select(pos.SelectionStart, pos.SelectionLength);
							richTextBoxSource.SelectionColor = Color.Green;
						}

					}
					else
					{
						//MessageBox.Show($"status {status},  data { data}");
					}

				}
			});
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
			if (string.IsNullOrWhiteSpace(richTextBoxSource.Text)) return;
			WorkAsync(new WorkAsyncInfo
			{

				Message = $"validating and then converting json ...",
				Work = (worker, args) =>
				{

					if (!string.IsNullOrWhiteSpace(richTextBoxSource.Text))
					{
						//validate json
						if (IsValidJson(richTextBoxSource.Text))
						{
							bool nest = false;
							string nameSpace = "";
							string classname = "RootClass";
							ICodeWriter writer = new CSharpCodeWriter();
							var gen = new JsonClassGenerator();
							gen.Example = richTextBoxSource.Text;
							gen.InternalVisibility = false;
							gen.CodeWriter = writer;
							gen.ExplicitDeserialization = false;
							if (nest)
								gen.Namespace = nameSpace;
							else
								gen.Namespace = null;

							gen.NoHelperClass = false;
							gen.SecondaryNamespace = null;
							gen.MainClass = classname;
							gen.UsePascalCase = true;
							gen.PropertyAttribute = "None";

							gen.UseNestedClasses = nest;
							gen.ApplyObfuscationAttributes = false;
							gen.SingleFile = true;
							gen.ExamplesInDocumentation = false;

							gen.TargetFolder = null;
							gen.SingleFile = true;
							using (var sw = new StringWriter())
							{
								gen.OutputStream = sw;
								gen.GenerateClasses();
								sw.WriteLine("// Root finalObject = JsonConvert.DeserializeObject<Root>(JsonResponse); ");
								sw.Flush();

								richTextBoxDest.Text = sw.ToString();
							}
							var pattern = @"public class|int|string|Guid|DateTime|bool|double|object|IList<[A-za-z*]*>";
							List<SelctionPattern> allselections = new List<SelctionPattern>();
							foreach (Match match in Regex.Matches(richTextBoxDest.Text, pattern))
							{
								allselections.Add(new SelctionPattern() { SelectionStart = match.Index, SelectionLength = match.Length, Text = match.Value });

							}
							foreach (var pos in allselections)
							{
								richTextBoxDest.Select(pos.SelectionStart, pos.SelectionLength);
								richTextBoxDest.SelectionColor = Color.Blue;
							}

						}
						else
						{
							MessageBox.Show("Invalid Json. Please input valid json string.");
							return;
						}

					}

				},
				PostWorkCallBack = (args) =>
				{
					if (args.Error != null)
					{
						MessageBox.Show(args.Error.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					}
					var result = args.Result as HttpResponseMessage;

					if (result != null && result.IsSuccessStatusCode)
					{
						var jsonaccountJson = result.Content.ReadAsStringAsync().Result;
						richTextBoxSource.Text = jsonaccountJson;

					}
					else
					{
						//MessageBox.Show($"status {status},  data { data}");
					}

				}
			});

		}

		private void cmbEntities_SelectedIndexChanged(object sender, EventArgs e)
		{
			var item = cmbEntities.SelectedItem as EntityModel;
			LoadNewJson(item.EntitySetName);
		}

		private void MyPluginControl_OnCloseTool_1(object sender, EventArgs e)
		{
			// Before leaving, save the settings
			SettingsManager.Instance.Save(GetType(), mySettings);
		}

		private void btnCopy_Click(object sender, EventArgs e)
		{
			if (!string.IsNullOrWhiteSpace(richTextBoxDest.Text))
			{
				Clipboard.SetText(richTextBoxDest.Text);
				MessageBox.Show($"Class copied to clipboard");
			}
		}

		private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
		{

		}

		private void MyPluginControl_Resize(object sender, EventArgs e)
		{
			Control control = (Control)sender;

			// Ensure the Form remains square (Height = Width).
			if (control.Size.Height != control.Size.Width)
			{
				control.Size = new Size(control.Size.Width, control.Size.Width);
				richTextBoxSource.Width = control.Width / 2;

			}
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