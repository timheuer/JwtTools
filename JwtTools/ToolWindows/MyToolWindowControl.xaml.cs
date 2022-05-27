using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using JwtTools.Helpers;
using System.Runtime.Remoting.Messaging;
using Microsoft.VisualStudio.Text.Editor;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.Utilities;
using System.Windows.Media;
using Microsoft.VisualStudio.Text;

namespace JwtTools
{
    public partial class MyToolWindowControl : UserControl
    {        
        public MyToolWindowControl()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(JwtToken.Text))
            {
                DecodedHeader.Content = string.Empty;
                DecodedPayload.Content = string.Empty;
                return;
            }

            try
            {
                JwtSecurityTokenHandler jwtHandler = new();
                var jwt = jwtHandler.ReadJwtToken(JwtToken.Text);

                ThreadHelper.ThrowIfNotOnUIThread();
                
                CreateTextView(JsonHelper.Format(jwt.Header.SerializeToJson(), Indentation.TwoSpaces), DecodedHeader);
                CreateTextView(JsonHelper.Format(jwt.Payload.SerializeToJson(), Indentation.TwoSpaces), DecodedPayload);
            }
            catch (Exception ex)
            {
                DecodedHeader.Content = ex.Message;
                DecodedPayload.Content = ex.Message;
            }
        }

        private void CreateTextView(string jsonString, ContentControl visualElement)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var componentModel = ServiceProvider.GlobalProvider.GetService<SComponentModel, IComponentModel>();
            if (componentModel is not null)
            {
                // Ensure services we'll use are loaded.
                var factory = componentModel.GetService<ITextEditorFactoryService>();
                var contentFactory = componentModel.GetService<IContentTypeRegistryService>();
                var roleSet = factory.CreateTextViewRoleSet(
                    PredefinedTextViewRoles.Interactive, 
                    PredefinedTextViewRoles.Structured, 
                    PredefinedTextViewRoles.PrimaryDocument, 
                    PredefinedTextViewRoles.Zoomable,
                    PredefinedTextViewRoles.Analyzable);
                var textBufferFactory = componentModel.GetService<ITextBufferFactoryService>();
                var textBuffer = textBufferFactory.CreateTextBuffer();
                var contentType = contentFactory.GetContentType("json");
                textBuffer.ChangeContentType(contentType, null);
                textBuffer.Insert(0, jsonString);

                var textView = factory.CreateTextView(textBuffer, roleSet);
                
                var textViewHost = factory.CreateTextViewHost(textView, true);

                //options
                textView.Options.SetOptionValue(DefaultTextViewHostOptions.EnableFileHealthIndicatorOptionId, false);
                textView.Options.SetOptionValue(DefaultTextViewHostOptions.LineNumberMarginId, true);
                textView.Options.SetOptionValue(DefaultTextViewHostOptions.GlyphMarginId, false);
                textView.Options.SetOptionValue(DefaultTextViewHostOptions.EditingStateMarginOptionId, false);
                textView.ZoomLevel = 100;

                visualElement.Content = textViewHost.HostControl;
            }
        }

        private void JwtToken_GotFocus(object sender, RoutedEventArgs e)
        {
            JwtToken.SelectAll();
        }
    }
}