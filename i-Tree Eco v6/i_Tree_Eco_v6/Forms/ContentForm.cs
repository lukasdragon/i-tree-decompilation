// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Forms.ContentForm
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using i_Tree_Eco_v6.Events;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using WeifenLuo.WinFormsUI.Docking;

namespace i_Tree_Eco_v6.Forms
{
  public class ContentForm : DockContent
  {
    protected internal Label lblBreadcrumb;
    private List<string> m_breadcrumbs;
    protected internal bool m_isDirty;
    protected int m_changes;

    public event EventHandler<ShowHelpEventArgs> ShowHelp;

    public event EventHandler<EventArgs> RequestRefresh;

    public ContentForm()
    {
      this.InitializeComponent();
      this.m_isDirty = false;
      this.FormClosing += new FormClosingEventHandler(this.ContentForm_FormClosing);
    }

    private void ContentForm_Layout(object sender, LayoutEventArgs e) => this.lblBreadcrumb.SendToBack();

    private void lblBreadcrumb_Paint(object sender, PaintEventArgs e)
    {
      if (this.m_breadcrumbs == null)
        return;
      GraphicsState gstate = e.Graphics.Save();
      Graphics graphics = e.Graphics;
      graphics.TextRenderingHint = TextRenderingHint.SingleBitPerPixel;
      string text1 = string.Join(" > ", (IEnumerable<string>) this.m_breadcrumbs).Trim();
      new StringFormat(StringFormat.GenericTypographic).FormatFlags |= StringFormatFlags.MeasureTrailingSpaces;
      TextFormatFlags flags = TextFormatFlags.WordBreak | TextFormatFlags.NoPadding;
      Size proposedSize = new Size(this.lblBreadcrumb.ClientSize.Width - 4, int.MaxValue);
      int y = (this.lblBreadcrumb.ClientSize.Height - TextRenderer.MeasureText((IDeviceContext) graphics, text1, this.lblBreadcrumb.Font, proposedSize, flags).Height) / 2;
      Size size1 = TextRenderer.MeasureText((IDeviceContext) graphics, " > ", this.lblBreadcrumb.Font, proposedSize, flags);
      int x1 = 4;
      Color foreColor = Color.FromArgb(0, 112, 192);
      SolidBrush solidBrush = new SolidBrush(Color.FromArgb(0, 112, 192));
      float val1 = 0.0f;
      for (int index = 0; index < this.m_breadcrumbs.Count; ++index)
      {
        Size size2 = TextRenderer.MeasureText((IDeviceContext) graphics, this.m_breadcrumbs[index], this.lblBreadcrumb.Font, proposedSize, flags);
        int width1 = size2.Width;
        int num = x1 + width1;
        size2 = this.lblBreadcrumb.ClientSize;
        int width2 = size2.Width;
        if (num > width2)
        {
          val1 = (float) (x1 + width1);
          y += size1.Height;
          x1 = 4;
        }
        string text2 = this.m_breadcrumbs[index].TrimStart('&');
        if (index < this.m_breadcrumbs.Count - 1)
        {
          TextRenderer.DrawText((IDeviceContext) graphics, text2, this.lblBreadcrumb.Font, new Point(x1, y), foreColor, flags);
          int x2 = x1 + width1;
          TextRenderer.DrawText((IDeviceContext) graphics, " > ", this.lblBreadcrumb.Font, new Point(x2, y), Color.SlateGray, flags);
          x1 = x2 + size1.Width;
        }
        else
        {
          TextRenderer.DrawText((IDeviceContext) graphics, text2, this.lblBreadcrumb.Font, new Point(x1, y), Color.DarkGreen, flags);
          val1 = Math.Max(val1, (float) (x1 + width1));
        }
      }
      graphics.DrawLine(Pens.LightGray, new PointF(0.0f, (float) (y + size1.Height)), new PointF(val1 * 1.2f, (float) (y + size1.Height)));
      graphics.Restore(gstate);
    }

    private void lblBreadcrumb_Layout(object sender, LayoutEventArgs e)
    {
      if (this.m_breadcrumbs == null)
        return;
      using (Graphics dc = Graphics.FromHwnd(this.lblBreadcrumb.Handle))
      {
        dc.TextRenderingHint = TextRenderingHint.SingleBitPerPixel;
        TextFormatFlags flags = TextFormatFlags.WordBreak | TextFormatFlags.NoPadding;
        Size proposedSize = new Size(this.lblBreadcrumb.ClientSize.Width - 4, int.MaxValue);
        string text1 = string.Join(" > ", (IEnumerable<string>) this.m_breadcrumbs).Trim();
        Size size = TextRenderer.MeasureText((IDeviceContext) dc, text1, this.lblBreadcrumb.Font, proposedSize, flags);
        StringFormat stringFormat = new StringFormat(StringFormat.GenericTypographic);
        stringFormat.FormatFlags |= StringFormatFlags.MeasureTrailingSpaces;
        Graphics graphics1 = dc;
        string text2 = text1;
        Font font1 = this.lblBreadcrumb.Font;
        Size clientSize = this.lblBreadcrumb.ClientSize;
        int width1 = clientSize.Width - 4;
        StringFormat format1 = stringFormat;
        graphics1.MeasureString(text2, font1, width1, format1);
        Graphics graphics2 = dc;
        Font font2 = this.lblBreadcrumb.Font;
        clientSize = this.lblBreadcrumb.ClientSize;
        int width2 = clientSize.Width;
        StringFormat format2 = stringFormat;
        graphics2.MeasureString(" ", font2, width2, format2);
        this.lblBreadcrumb.Height = size.Height + 25;
      }
    }

    private void ContentForm_Move(object sender, EventArgs e)
    {
      if (this.m_breadcrumbs == null)
        return;
      this.lblBreadcrumb.Invalidate();
    }

    private void AnyControl_Changed(object sender, EventArgs e)
    {
      if (this.m_changes != 0)
        return;
      this.m_isDirty = true;
    }

    private void ContentForm_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (!this.m_isDirty)
        return;
      DialogResult dialogResult = MessageBox.Show((IWin32Window) this, i_Tree_Eco_v6.Resources.Strings.MsgConfirmClose, Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2);
      e.Cancel = dialogResult != DialogResult.Yes;
    }

    private void InitializeComponent()
    {
      this.lblBreadcrumb = new Label();
      this.SuspendLayout();
      this.lblBreadcrumb.Dock = DockStyle.Top;
      this.lblBreadcrumb.Font = new Font("Calibri", 14f);
      this.lblBreadcrumb.Location = new Point(0, 0);
      this.lblBreadcrumb.Margin = new Padding(5);
      this.lblBreadcrumb.Name = "lblBreadcrumb";
      this.lblBreadcrumb.Size = new Size(486, 29);
      this.lblBreadcrumb.TabIndex = 0;
      this.lblBreadcrumb.TextAlign = ContentAlignment.MiddleLeft;
      this.lblBreadcrumb.Paint += new PaintEventHandler(this.lblBreadcrumb_Paint);
      this.lblBreadcrumb.Layout += new LayoutEventHandler(this.lblBreadcrumb_Layout);
      this.lblBreadcrumb.Resize += new EventHandler(this.lblBreadcrumb_Resize);
      this.BackColor = Color.White;
      this.ClientSize = new Size(486, 261);
      this.Controls.Add((Control) this.lblBreadcrumb);
      this.Font = new Font("Calibri", 11.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.Name = nameof (ContentForm);
      this.Layout += new LayoutEventHandler(this.ContentForm_Layout);
      this.Move += new EventHandler(this.ContentForm_Move);
      this.ResumeLayout(false);
    }

    protected internal List<string> Breadcrumbs
    {
      get => this.m_breadcrumbs;
      set
      {
        this.m_breadcrumbs = value;
        this.lblBreadcrumb.PerformLayout();
        this.lblBreadcrumb.Invalidate();
      }
    }

    protected virtual void OnShowHelp(string topic)
    {
      EventHandler<ShowHelpEventArgs> showHelp = this.ShowHelp;
      if (showHelp == null)
        return;
      Delegate[] invocationList = showHelp.GetInvocationList();
      ShowHelpEventArgs showHelpEventArgs = new ShowHelpEventArgs()
      {
        Topic = topic
      };
      foreach (Delegate method in invocationList)
      {
        Control target = method.Target as Control;
        if (target.InvokeRequired)
          target.Invoke(method, (object) this, (object) showHelpEventArgs);
        else
          method.DynamicInvoke((object) this, (object) showHelpEventArgs);
      }
    }

    protected virtual void OnRequestRefresh()
    {
      EventHandler<EventArgs> requestRefresh = this.RequestRefresh;
      if (requestRefresh == null)
        return;
      foreach (EventHandler<EventArgs> invocation in requestRefresh.GetInvocationList())
      {
        Control target = invocation.Target as Control;
        if (target.InvokeRequired)
          target.Invoke((Delegate) invocation, (object) this, (object) EventArgs.Empty);
        else
          invocation((object) this, EventArgs.Empty);
      }
    }

    protected void registerChangeEvents(Control ctrl)
    {
      Stack<Control> controlStack = new Stack<Control>();
      for (; ctrl != null; ctrl = controlStack.Count <= 0 ? (Control) null : controlStack.Pop())
      {
        if (ctrl.HasChildren)
        {
          foreach (Control control in (ArrangedElementCollection) ctrl.Controls)
            controlStack.Push(control);
        }
        switch (ctrl)
        {
          case TextBoxBase _:
            (ctrl as TextBoxBase).TextChanged += new EventHandler(this.AnyControl_Changed);
            break;
          case CheckBox _:
            (ctrl as CheckBox).CheckedChanged += new EventHandler(this.AnyControl_Changed);
            break;
          case RadioButton _:
            (ctrl as RadioButton).CheckedChanged += new EventHandler(this.AnyControl_Changed);
            break;
          case ListControl _:
            (ctrl as ListControl).SelectedValueChanged += new EventHandler(this.AnyControl_Changed);
            break;
          case NumericUpDown _:
            (ctrl as NumericUpDown).ValueChanged += new EventHandler(this.AnyControl_Changed);
            break;
        }
      }
    }

    private void lblBreadcrumb_Resize(object sender, EventArgs e) => this.lblBreadcrumb.Invalidate();
  }
}
