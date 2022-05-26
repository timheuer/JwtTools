using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using JwtTools.Helpers;

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
            JwtSecurityTokenHandler jwtHandler = new();
            var jwt = jwtHandler.ReadJwtToken(JwtToken.Text);

            DecodedHeader.Text = JsonHelper.Format(jwt.Header.SerializeToJson(), Indentation.TwoSpaces);
            DecodedPayload.Text = JsonHelper.Format(jwt.Payload.SerializeToJson(), Indentation.TwoSpaces);
        }
    }
}