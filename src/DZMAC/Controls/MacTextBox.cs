using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Dzmac.Controls
{
    /// <summary>
    ///     Reference: https://www.codeproject.com/Articles/15117/MAC-Address-Text-Box-and-Class
    /// </summary>
    internal partial class MacTextBox : UserControl
    {
        private readonly KeyPressEventArgs KeyPressBuffer = new KeyPressEventArgs((char)0);

        public MacTextBox()
        {
            InitializeComponent();

            // make the box a size of (171, 24) when loading initially
            Size = new Size(171, 24);

            // initialize zeros for the boxes
            Box1!.Text = "00";
            Box2!.Text = "00";
            Box3!.Text = "00";
            Box4!.Text = "00";
            Box5!.Text = "00";
            Box6!.Text = "00";
        }

        [Browsable(true)]
        public override Color BackColor {
            get => base.BackColor;
            set {
                Box1!.BackColor = value;
                Box2!.BackColor = value;
                Box3!.BackColor = value;
                Box4!.BackColor = value;
                Box5!.BackColor = value;
                Box6!.BackColor = value;
                base.BackColor = value;
            }
        }

        [Browsable(true)]
        public override Color ForeColor {
            get => base.ForeColor;
            set {
                Box1!.ForeColor = value;
                Box2!.ForeColor = value;
                Box3!.ForeColor = value;
                Box4!.ForeColor = value;
                Box5!.ForeColor = value;
                Box6!.ForeColor = value;
                base.ForeColor = value;
            }
        }

        public override string Text {
            get => string.Format("{0}-{1}-{2}-{3}-{4}-{5}", Box1!.Text, Box2!.Text, Box3!.Text, Box4!.Text, Box5!.Text, Box6!.Text);
            set {
                if (value != "" && value is not null)
                {
                    var pieces = value.Contains("-") ? value.Split("-".ToCharArray(), 6) : value.Split(":".ToCharArray(), 6);
                    Box1!.Text = pieces[0];
                    Box2!.Text = pieces[1];
                    Box3!.Text = pieces[2];
                    Box4!.Text = pieces[3];
                    Box5!.Text = pieces[4];
                    Box6!.Text = pieces[5];
                }
                else
                {
                    Box1!.Text = "00";
                    Box2!.Text = "00";
                    Box3!.Text = "00";
                    Box4!.Text = "00";
                    Box5!.Text = "00";
                    Box6!.Text = "00";
                }
            }
        }

        private bool IsValidChar(char c)
        {
            if (c >= 'a' && c <= 'f')
            {
                return true;
            }

            if (c >= 'A' && c <= 'F')
            {
                return true;
            }

            return false;
        }

        private void Box1_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Only Accept a ':', a '-', a numeral, a - f / A - F, or backspace
            if (IsValidChar(e.KeyChar) || e.KeyChar.ToString() == ":" || e.KeyChar.ToString() == "-" || char.IsDigit(e.KeyChar) || e.KeyChar == 8)
            {
                //If the key pressed is a ':' or '-'
                if (e.KeyChar.ToString() == ":" || e.KeyChar.ToString() == "-")
                {
                    //If the Text is valid move to the next box
                    if (Box1!.Text != "" && Box1.Text.Length != Box1.SelectionLength)
                    {
                        Box2!.Focus();
                    }
                    e.Handled = true;
                }

                //If we are not overwriting the whole text
                else if (Box1!.SelectionLength != Box1.Text.Length)
                {
                    //Check that the new Text value will be valid
                    // then move on to next box
                    if (Box1.Text.Length == 1)
                    {
                        if (e.KeyChar != 8)
                        {
                            KeyPressBuffer.KeyChar = e.KeyChar;
                            Box2!.Focus();
                        }
                    }
                }
            }
            //Do nothing if the keypress is not a hex value, backspace, '-', or ':'
            else
            {
                e.Handled = true;
            }
        }

        private void Box2_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Only Accept a ':', a '-', a numeral, a - f / A - F, or backspace
            if (IsValidChar(e.KeyChar) || e.KeyChar.ToString() == ":" || e.KeyChar.ToString() == "-" || char.IsDigit(e.KeyChar) || e.KeyChar == 8)
            {
                //If the key pressed is a ':' or '-'
                if (e.KeyChar.ToString() == ":" || e.KeyChar.ToString() == "-")
                {
                    //If the Text is valid move to the next box
                    if (Box2!.Text != "" && Box2.Text.Length != Box2.SelectionLength)
                    {
                        Box3!.Focus();
                    }
                    e.Handled = true;
                }

                //If we are not overwriting the whole text
                else if (Box2!.SelectionLength != Box2.Text.Length)
                {
                    //Check that the new Text value will be valid
                    // then move on to next box
                    if (Box2.Text.Length == 1)
                    {
                        if (e.KeyChar != 8)
                        {
                            KeyPressBuffer.KeyChar = e.KeyChar;
                            Box3!.Focus();
                        }
                    }
                }
            }
            //Do nothing if the keypress is not a hex value, backspace, '-', or ':'
            else
            {
                e.Handled = true;
            }
        }

        private void Box3_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Only Accept a ':', a '-', a numeral, a - f / A - F, or backspace
            if (IsValidChar(e.KeyChar) || e.KeyChar.ToString() == ":" || e.KeyChar.ToString() == "-" || char.IsDigit(e.KeyChar) || e.KeyChar == 8)
            {
                //If the key pressed is a ':' or '-'
                if (e.KeyChar.ToString() == ":" || e.KeyChar.ToString() == "-")
                {
                    //If the Text is valid move to the next box
                    if (Box3!.Text != "" && Box3.Text.Length != Box3.SelectionLength)
                    {
                        Box4!.Focus();
                    }
                    e.Handled = true;
                }

                //If we are not overwriting the whole text
                else if (Box3!.SelectionLength != Box3.Text.Length)
                {
                    //Check that the new Text value will be valid
                    // then move on to next box
                    if (Box3.Text.Length == 1)
                    {
                        if (e.KeyChar != 8)
                        {
                            KeyPressBuffer.KeyChar = e.KeyChar;
                            Box4!.Focus();
                        }
                    }
                }
            }
            //Do nothing if the keypress is not a hex value, backspace, '-', or ':'
            else
            {
                e.Handled = true;
            }
        }

        private void Box4_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Only Accept a ':', a '-', a numeral, a - f / A - F, or backspace
            if (IsValidChar(e.KeyChar) || e.KeyChar.ToString() == ":" || e.KeyChar.ToString() == "-" || char.IsDigit(e.KeyChar) || e.KeyChar == 8)
            {
                //If the key pressed is a ':' or '-'
                if (e.KeyChar.ToString() == ":" || e.KeyChar.ToString() == "-")
                {
                    //If the Text is valid move to the next box
                    if (Box4!.Text != "" && Box4.Text.Length != Box4.SelectionLength)
                    {
                        Box5!.Focus();
                    }
                    e.Handled = true;
                }

                //If we are not overwriting the whole text
                else if (Box4!.SelectionLength != Box4.Text.Length)
                {
                    //Check that the new Text value will be valid
                    // then move on to next box
                    if (Box4.Text.Length == 1)
                    {
                        if (e.KeyChar != 8)
                        {
                            KeyPressBuffer.KeyChar = e.KeyChar;
                            Box5!.Focus();
                        }
                    }
                }
            }
            //Do nothing if the keypress is not a hex value, backspace, '-', or ':'
            else
            {
                e.Handled = true;
            }
        }

        private void Box5_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Only Accept a ':', a '-', a numeral, a - f / A - F, or backspace
            if (IsValidChar(e.KeyChar) || e.KeyChar.ToString() == ":" || e.KeyChar.ToString() == "-" || char.IsDigit(e.KeyChar) || e.KeyChar == 8)
            {
                //If the key pressed is a ':' or '-'
                if (e.KeyChar.ToString() == ":" || e.KeyChar.ToString() == "-")
                {
                    //If the Text is valid move to the next box
                    if (Box5!.Text != "" && Box5.Text.Length != Box5.SelectionLength)
                    {
                        Box6!.Focus();
                    }
                    e.Handled = true;
                }

                //If we are not overwriting the whole text
                else if (Box5!.SelectionLength != Box5.Text.Length)
                {
                    //Check that the new Text value will be valid
                    // then move on to next box
                    if (Box5.Text.Length == 1)
                    {
                        if (e.KeyChar != 8)
                        {
                            KeyPressBuffer.KeyChar = e.KeyChar;
                            Box6!.Focus();
                        }
                    }
                }
            }
            //Do nothing if the keypress is not a hex value, backspace, '-', or ':'
            else
            {
                e.Handled = true;
            }
        }

        private void Box6_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Similar to Box5 but ignores the ':' and '-' characters and does not advance
            //to the next box.  Also Box4 is previous box for backspace case.
            if (IsValidChar(e.KeyChar) || char.IsDigit(e.KeyChar) || e.KeyChar == 8)
            {
                if (Box6!.SelectionLength != Box6.Text.Length)
                {
                    if (Box6.Text.Length == 2)
                    {
                        if (e.KeyChar == 8)
                        {
                            Box6.Text.Remove(Box6.Text.Length - 1, 1);
                        }
                    }
                }
                else if (Box6.Text.Length == 0 && e.KeyChar == 8)
                {
                    Box5!.Focus();
                    Box5.SelectionStart = Box5.Text.Length;
                }
            }
            else
            {
                e.Handled = true;
            }
        }

        private void MacTextBox_EnabledChanged(object sender, EventArgs e)
        {
            var lbl = (UserControl)sender;
            lbl.BackColor = lbl.Enabled ? SystemColors.Window : SystemColors.Control;
        }

        private void Box_Enter(object sender, EventArgs e)
        {
            var tb = (TextBox)sender;
            tb.SelectAll();
        }

        private void Label1_EnabledChanged(object sender, EventArgs e)
        {
            var lbl = (Label)sender;
            lbl.BackColor = lbl.Enabled ? SystemColors.Window : SystemColors.Control;
        }

        private void Box_Exit(object sender, EventArgs e)
        {
            var tb = (TextBox)sender;
            if (tb.Text.Length <= 2)
            {
                // do not pad if KeyPressBuffer has valid hex value waiting to be written to the text
                if (!IsValidChar(KeyPressBuffer.KeyChar) && !(KeyPressBuffer.KeyChar >= '0' && KeyPressBuffer.KeyChar <= '9'))
                {
                    tb.Text = tb.Text.PadLeft(2, '0');
                }
            }
            // clear buffer
            KeyPressBuffer.KeyChar = (char)0;
        }
    }
}