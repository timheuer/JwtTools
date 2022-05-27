using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using JwtTools.Helpers;
using System.Runtime.Remoting.Messaging;

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
                DecodedHeader.Text = string.Empty;
                DecodedPayload.Text = string.Empty;
                return;
            }

            try
            {
                JwtSecurityTokenHandler jwtHandler = new();
                var jwt = jwtHandler.ReadJwtToken(JwtToken.Text);

                DecodedHeader.Text = JsonHelper.Format(jwt.Header.SerializeToJson(), Indentation.TwoSpaces);
                DecodedPayload.Text = JsonHelper.Format(jwt.Payload.SerializeToJson(), Indentation.TwoSpaces);
            }
            catch (Exception ex)
            {
                DecodedHeader.Text = ex.Message;
                DecodedPayload.Text = ex.Message;
            }
        }

        private void JwtToken_GotFocus(object sender, RoutedEventArgs e)
        {
            JwtToken.SelectAll();
        }
    }
}