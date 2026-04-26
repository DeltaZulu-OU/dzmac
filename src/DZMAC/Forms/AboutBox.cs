using System;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using Dzmac.Core;

namespace Dzmac.Forms
{
    [DebuggerDisplay("{" + nameof(GetDebuggerDisplay) + "(),nq}")]
    internal partial class AboutBox : Form
    {
        public AboutBox()
        {
            InitializeComponent();
            Icon = AppIconProvider.GetIcon();

            var productName = AssemblyProduct;
            Text = $"About {productName}";
            ProductNameLabel.Text = $"   {productName}";
            ProductNameLabel.Image = AppIconProvider.GetBitmap(new Size(16, 16));
            ProductNameLabel.ImageAlign = ContentAlignment.MiddleLeft;
            ProductNameLabel.Padding = new Padding(0, 0, 0, 0);
            VersionLabel.Text = $"Version {AssemblyVersion}";
            CopyrightLabel.Text = AssemblyCopyright;
            DescriptionLabel.Text = AssemblyDescription;
        }

        #region Assembly Attribute Accessors

        public string AssemblyVersion => Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "Unknown";

        public string AssemblyDescription
        {
            get
            {
                var attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
                if (attributes.Length == 0)
                {
                    return "Change and manage network adapter MAC addresses.";
                }

                var description = ((AssemblyDescriptionAttribute)attributes[0]).Description;
                return string.IsNullOrWhiteSpace(description)
                    ? "Change and manage network adapter MAC addresses."
                    : description;
            }
        }

        public string AssemblyProduct
        {
            get
            {
                var attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);
                if (attributes.Length == 0)
                {
                    return "DZMAC";
                }

                var product = ((AssemblyProductAttribute)attributes[0]).Product;
                return string.IsNullOrWhiteSpace(product) ? "DZMAC" : product;
            }
        }

        public string AssemblyCopyright
        {
            get
            {
                var attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }

                return ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
            }
        }

        #endregion Assembly Attribute Accessors

        private void CloseButton_Click(object sender, EventArgs e) => Close();

        private void ProjectLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ProjectLinkLabel.LinkVisited = true;
            Process.Start("https://github.com/DeltaZulu-OU/dzmac");
        }

        private string GetDebuggerDisplay() => "About";
    }
}
