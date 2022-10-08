// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.SAS.SelectSpecies
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using Eco.Util.Views;
using LocationSpecies.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace i_Tree_Eco_v6.SAS
{
  public class SelectSpecies : Form
  {
    public string SelectedSpeciesCode = "";
    private ProgramSession _m_ps;
    private int _minimumChars = 3;
    private bool _genusOnly;
    private IContainer components;
    private Label labelFromSpecies;
    private Label labelSpecies;
    private ComboBox cboSpecies;
    private Label label1;
    private Label labelToSpecies;
    private Panel panel1;
    private Button btnOK;
    private Button btnCancel;
    private Panel panel2;
    private Panel panel3;

    public SelectSpecies() => this.InitializeComponent();

    public void InitializeForm(
      ProgramSession m_ps,
      string fromSpeciesDisplay,
      string toSpeciesDisplay,
      bool GenusOnly)
    {
      this._m_ps = m_ps;
      this._genusOnly = GenusOnly;
      if (this._genusOnly)
        this._minimumChars = 2;
      this.labelFromSpecies.Text = fromSpeciesDisplay;
      this.labelToSpecies.Text = toSpeciesDisplay;
      this.labelSpecies.Text = string.Format("Select a different {0} to replace by typing {1} or more letters below:", this._genusOnly ? (object) "genus" : (object) "species", (object) this._minimumChars);
      this.cboSpecies.Tag = (object) "";
      this.cboSpecies.Focus();
    }

    private void Species_KeyUp(object sender, KeyEventArgs e)
    {
      if (this.cboSpecies.Text.Length < this._minimumChars || string.IsNullOrEmpty(this.cboSpecies.Tag.ToString()))
      {
        for (int index = this.cboSpecies.Items.Count - 1; index >= 0; --index)
          this.cboSpecies.Items.RemoveAt(index);
        this.cboSpecies.Tag = (object) "";
      }
      if (this.cboSpecies.Text.Length < this._minimumChars)
        return;
      if (string.IsNullOrEmpty(this.cboSpecies.Tag.ToString()) || this.cboSpecies.Text.IndexOf(this.cboSpecies.Tag.ToString(), StringComparison.OrdinalIgnoreCase) < 0)
      {
        foreach (KeyValuePair<string, SpeciesView> specy in this._m_ps.Species)
        {
          if (!this._genusOnly || this._genusOnly && specy.Value.Rank == SpeciesRank.Genus)
          {
            string str = specy.Key + " - " + specy.Value.CommonScientificName;
            if (str.IndexOf(this.cboSpecies.Text, StringComparison.OrdinalIgnoreCase) >= 0)
              this.cboSpecies.Items.Add((object) str);
          }
        }
        this.cboSpecies.Tag = (object) this.cboSpecies.Text;
      }
      else
      {
        for (int index = this.cboSpecies.Items.Count - 1; index >= 0; --index)
        {
          if (this.cboSpecies.Items[index].ToString().IndexOf(this.cboSpecies.Text, StringComparison.OrdinalIgnoreCase) < 0)
            this.cboSpecies.Items.RemoveAt(index);
        }
        this.cboSpecies.Tag = (object) this.cboSpecies.Text;
      }
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
      if (string.IsNullOrEmpty(this.cboSpecies.Text.Trim()))
        return;
      if (this.cboSpecies.SelectedIndex == -1)
      {
        int num = (int) MessageBox.Show("The species is invalid.");
      }
      else
      {
        this.SelectedSpeciesCode = this.cboSpecies.Text.Substring(0, this.cboSpecies.Text.IndexOf(" - ")).Trim();
        this.Hide();
      }
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
      this.SelectedSpeciesCode = "";
      this.Hide();
    }

    private void form_activated(object sender, EventArgs e)
    {
    }

    private void SelectSpecies_Load(object sender, EventArgs e)
    {
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.labelFromSpecies = new Label();
      this.labelSpecies = new Label();
      this.cboSpecies = new ComboBox();
      this.label1 = new Label();
      this.labelToSpecies = new Label();
      this.panel1 = new Panel();
      this.btnOK = new Button();
      this.btnCancel = new Button();
      this.panel2 = new Panel();
      this.panel3 = new Panel();
      this.panel1.SuspendLayout();
      this.panel2.SuspendLayout();
      this.panel3.SuspendLayout();
      this.SuspendLayout();
      this.labelFromSpecies.AutoSize = true;
      this.labelFromSpecies.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.labelFromSpecies.Location = new Point(6, 14);
      this.labelFromSpecies.Name = "labelFromSpecies";
      this.labelFromSpecies.Size = new Size(35, 13);
      this.labelFromSpecies.TabIndex = 0;
      this.labelFromSpecies.Text = "label1";
      this.labelSpecies.AutoSize = true;
      this.labelSpecies.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.labelSpecies.Location = new Point(6, 94);
      this.labelSpecies.Name = "labelSpecies";
      this.labelSpecies.Size = new Size(236, 13);
      this.labelSpecies.TabIndex = 1;
      this.labelSpecies.Text = "Select a different species by typing letters below:";
      this.cboSpecies.FormattingEnabled = true;
      this.cboSpecies.Location = new Point(3, 110);
      this.cboSpecies.Name = "cboSpecies";
      this.cboSpecies.Size = new Size(518, 21);
      this.cboSpecies.TabIndex = 3;
      this.cboSpecies.KeyUp += new KeyEventHandler(this.Species_KeyUp);
      this.label1.AutoSize = true;
      this.label1.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label1.Location = new Point(48, 34);
      this.label1.Name = "label1";
      this.label1.Size = new Size(94, 13);
      this.label1.TabIndex = 6;
      this.label1.Text = "will be replaced by";
      this.labelToSpecies.AutoSize = true;
      this.labelToSpecies.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.labelToSpecies.Location = new Point(10, 53);
      this.labelToSpecies.Name = "labelToSpecies";
      this.labelToSpecies.Size = new Size(35, 13);
      this.labelToSpecies.TabIndex = 7;
      this.labelToSpecies.Text = "label3";
      this.panel1.Controls.Add((Control) this.btnOK);
      this.panel1.Controls.Add((Control) this.btnCancel);
      this.panel1.Dock = DockStyle.Right;
      this.panel1.Location = new Point(266, 0);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(277, 61);
      this.panel1.TabIndex = 8;
      this.btnOK.Location = new Point(19, 16);
      this.btnOK.Name = "btnOK";
      this.btnOK.RightToLeft = RightToLeft.No;
      this.btnOK.Size = new Size(109, 34);
      this.btnOK.TabIndex = 6;
      this.btnOK.Text = "OK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.btnCancel.Location = new Point(153, 16);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.RightToLeft = RightToLeft.No;
      this.btnCancel.Size = new Size(109, 34);
      this.btnCancel.TabIndex = 5;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
      this.panel2.Controls.Add((Control) this.panel1);
      this.panel2.Dock = DockStyle.Bottom;
      this.panel2.Location = new Point(0, 180);
      this.panel2.Name = "panel2";
      this.panel2.Size = new Size(543, 61);
      this.panel2.TabIndex = 9;
      this.panel3.Controls.Add((Control) this.labelToSpecies);
      this.panel3.Controls.Add((Control) this.label1);
      this.panel3.Controls.Add((Control) this.cboSpecies);
      this.panel3.Controls.Add((Control) this.labelSpecies);
      this.panel3.Controls.Add((Control) this.labelFromSpecies);
      this.panel3.Dock = DockStyle.Fill;
      this.panel3.Location = new Point(0, 0);
      this.panel3.Name = "panel3";
      this.panel3.Size = new Size(543, 180);
      this.panel3.TabIndex = 10;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(543, 241);
      this.ControlBox = false;
      this.Controls.Add((Control) this.panel3);
      this.Controls.Add((Control) this.panel2);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.Name = nameof (SelectSpecies);
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Select Species";
      this.Activated += new EventHandler(this.form_activated);
      this.Load += new EventHandler(this.SelectSpecies_Load);
      this.panel1.ResumeLayout(false);
      this.panel2.ResumeLayout(false);
      this.panel3.ResumeLayout(false);
      this.panel3.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
