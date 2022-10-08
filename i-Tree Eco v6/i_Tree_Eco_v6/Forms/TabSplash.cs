// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Forms.TabSplash
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using DaveyTree.Controls;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace i_Tree_Eco_v6.Forms
{
  public class TabSplash : DockContent
  {
    private IContainer components;
    private PictureBox pbImage;
    private RichTextLabel rtlTabText;

    public TabSplash(Image tabImage, string Title, string tabText, bool IsTextRTF)
    {
      this.InitializeComponent();
      if (tabImage != null)
      {
        this.pbImage.Image = tabImage;
        this.pbImage.Width = tabImage.Width;
      }
      else
        this.pbImage.Width = 0;
      this.Text = Title;
      if (IsTextRTF)
        this.rtlTabText.RichText = tabText;
      else
        this.rtlTabText.Text = tabText;
      HighDpiHelper.AdjustControlImagesDpiScale((Control) this);
    }

    private void rtlTabText_Resize(object sender, EventArgs e)
    {
      int num = this.rtlTabText.Size.Width - SystemInformation.VerticalScrollBarWidth - 10;
      if (num > 0)
        this.rtlTabText.RightMargin = num;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.pbImage = new PictureBox();
      this.rtlTabText = new RichTextLabel();
      ((ISupportInitialize) this.pbImage).BeginInit();
      this.SuspendLayout();
      this.pbImage.BackColor = Color.White;
      this.pbImage.Dock = DockStyle.Left;
      this.pbImage.Image = (Image) i_Tree_Eco_v6.Properties.Resources.iTreeSidebar;
      this.pbImage.Location = new Point(0, 0);
      this.pbImage.Margin = new Padding(0);
      this.pbImage.Name = "pbImage";
      this.pbImage.Size = new Size(161, 363);
      this.pbImage.SizeMode = PictureBoxSizeMode.AutoSize;
      this.pbImage.TabIndex = 0;
      this.pbImage.TabStop = false;
      this.rtlTabText.BackgroundImageLayout = ImageLayout.None;
      this.rtlTabText.Dock = DockStyle.Fill;
      this.rtlTabText.Location = new Point(161, 0);
      this.rtlTabText.Name = "rtlTabText";
      this.rtlTabText.RichText = "{\\rtf1\\ansi\\ansicpg1252\\deff0\\nouicompat\\deflang1033{\\fonttbl{\\f0\\fnil\\fcharset0 Microsoft Sans Serif;}}\r\n{\\*\\generator Riched20 10.0.18362}\\viewkind4\\uc1 \r\n\\pard\\f0\\fs17\\par\r\n}\r\n";
      this.rtlTabText.ScrollBars = RichTextBoxScrollBars.Vertical;
      this.rtlTabText.Size = new Size(303, 363);
      this.rtlTabText.TabIndex = 2;
      this.rtlTabText.TabStop = false;
      this.rtlTabText.Resize += new EventHandler(this.rtlTabText_Resize);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.White;
      this.ClientSize = new Size(464, 363);
      this.Controls.Add((Control) this.rtlTabText);
      this.Controls.Add((Control) this.pbImage);
      this.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.Name = nameof (TabSplash);
      this.Text = nameof (TabSplash);
      ((ISupportInitialize) this.pbImage).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
