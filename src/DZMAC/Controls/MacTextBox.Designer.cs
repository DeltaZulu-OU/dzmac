#nullable enable

namespace Dzmac.Controls
{
    internal partial class MacTextBox
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer? components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components is not null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Box6 = new System.Windows.Forms.TextBox();
            this.Label5 = new System.Windows.Forms.Label();
            this.Box5 = new System.Windows.Forms.TextBox();
            this.Box4 = new System.Windows.Forms.TextBox();
            this.Box3 = new System.Windows.Forms.TextBox();
            this.Box2 = new System.Windows.Forms.TextBox();
            this.Box1 = new System.Windows.Forms.TextBox();
            this.Label4 = new System.Windows.Forms.Label();
            this.Label3 = new System.Windows.Forms.Label();
            this.Label2 = new System.Windows.Forms.Label();
            this.Label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // Box6
            // 
            this.Box6.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.Box6.Location = new System.Drawing.Point(146, 3);
            this.Box6.MaxLength = 2;
            this.Box6.Name = "Box6";
            this.Box6.Size = new System.Drawing.Size(16, 13);
            this.Box6.TabIndex = 21;
            this.Box6.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Box6.Enter += new System.EventHandler(this.Box_Enter);
            this.Box6.Leave += new System.EventHandler(this.Box_Exit);
            this.Box6.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Box6_KeyPress);
            // 
            // Label5
            // 
            this.Label5.Location = new System.Drawing.Point(136, 3);
            this.Label5.Name = "Label5";
            this.Label5.Size = new System.Drawing.Size(8, 13);
            this.Label5.TabIndex = 20;
            this.Label5.Text = ":";
            this.Label5.EnabledChanged += new System.EventHandler(this.Label1_EnabledChanged);
            // 
            // Box5
            // 
            this.Box5.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.Box5.Location = new System.Drawing.Point(117, 3);
            this.Box5.MaxLength = 2;
            this.Box5.Name = "Box5";
            this.Box5.Size = new System.Drawing.Size(16, 13);
            this.Box5.TabIndex = 19;
            this.Box5.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Box5.Enter += new System.EventHandler(this.Box_Enter);
            this.Box5.Leave += new System.EventHandler(this.Box_Exit);
            this.Box5.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Box5_KeyPress);
            // 
            // Box4
            // 
            this.Box4.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.Box4.Location = new System.Drawing.Point(89, 3);
            this.Box4.MaxLength = 2;
            this.Box4.Name = "Box4";
            this.Box4.Size = new System.Drawing.Size(16, 13);
            this.Box4.TabIndex = 17;
            this.Box4.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Box4.Enter += new System.EventHandler(this.Box_Enter);
            this.Box4.Leave += new System.EventHandler(this.Box_Exit);
            this.Box4.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Box4_KeyPress);
            // 
            // Box3
            // 
            this.Box3.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.Box3.Location = new System.Drawing.Point(60, 3);
            this.Box3.MaxLength = 2;
            this.Box3.Name = "Box3";
            this.Box3.Size = new System.Drawing.Size(16, 13);
            this.Box3.TabIndex = 15;
            this.Box3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Box3.Enter += new System.EventHandler(this.Box_Enter);
            this.Box3.Leave += new System.EventHandler(this.Box_Exit);
            this.Box3.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Box3_KeyPress);
            // 
            // Box2
            // 
            this.Box2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.Box2.Location = new System.Drawing.Point(32, 3);
            this.Box2.MaxLength = 2;
            this.Box2.Name = "Box2";
            this.Box2.Size = new System.Drawing.Size(16, 13);
            this.Box2.TabIndex = 13;
            this.Box2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Box2.Enter += new System.EventHandler(this.Box_Enter);
            this.Box2.Leave += new System.EventHandler(this.Box_Exit);
            this.Box2.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Box2_KeyPress);
            // 
            // Box1
            // 
            this.Box1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.Box1.Location = new System.Drawing.Point(3, 3);
            this.Box1.MaxLength = 2;
            this.Box1.Name = "Box1";
            this.Box1.Size = new System.Drawing.Size(16, 13);
            this.Box1.TabIndex = 11;
            this.Box1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Box1.Enter += new System.EventHandler(this.Box_Enter);
            this.Box1.Leave += new System.EventHandler(this.Box_Exit);
            this.Box1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Box1_KeyPress);
            // 
            // Label4
            // 
            this.Label4.Location = new System.Drawing.Point(107, 3);
            this.Label4.Name = "Label4";
            this.Label4.Size = new System.Drawing.Size(8, 13);
            this.Label4.TabIndex = 18;
            this.Label4.Text = ":";
            this.Label4.EnabledChanged += new System.EventHandler(this.Label1_EnabledChanged);
            // 
            // Label3
            // 
            this.Label3.Location = new System.Drawing.Point(78, 3);
            this.Label3.Name = "Label3";
            this.Label3.Size = new System.Drawing.Size(8, 13);
            this.Label3.TabIndex = 16;
            this.Label3.Text = ":";
            this.Label3.EnabledChanged += new System.EventHandler(this.Label1_EnabledChanged);
            // 
            // Label2
            // 
            this.Label2.Location = new System.Drawing.Point(49, 3);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(8, 13);
            this.Label2.TabIndex = 14;
            this.Label2.Text = ":";
            this.Label2.EnabledChanged += new System.EventHandler(this.Label1_EnabledChanged);
            // 
            // Label1
            // 
            this.Label1.Location = new System.Drawing.Point(21, 3);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(8, 13);
            this.Label1.TabIndex = 12;
            this.Label1.Text = ":";
            this.Label1.EnabledChanged += new System.EventHandler(this.Label1_EnabledChanged);
            // 
            // matb
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Controls.Add(this.Box6);
            this.Controls.Add(this.Label5);
            this.Controls.Add(this.Box5);
            this.Controls.Add(this.Box4);
            this.Controls.Add(this.Box3);
            this.Controls.Add(this.Box2);
            this.Controls.Add(this.Box1);
            this.Controls.Add(this.Label4);
            this.Controls.Add(this.Label3);
            this.Controls.Add(this.Label2);
            this.Controls.Add(this.Label1);
            this.Name = "matb";
            this.Size = new System.Drawing.Size(169, 24);
            this.EnabledChanged += new System.EventHandler(this.MacTextBox_EnabledChanged);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox? Box6;
        private System.Windows.Forms.Label? Label5;
        private System.Windows.Forms.TextBox? Box5;
        private System.Windows.Forms.TextBox? Box4;
        private System.Windows.Forms.TextBox? Box3;
        private System.Windows.Forms.TextBox? Box2;
        private System.Windows.Forms.TextBox? Box1;
        private System.Windows.Forms.Label? Label4;
        private System.Windows.Forms.Label? Label3;
        private System.Windows.Forms.Label? Label2;
        private System.Windows.Forms.Label? Label1;
    }
}
