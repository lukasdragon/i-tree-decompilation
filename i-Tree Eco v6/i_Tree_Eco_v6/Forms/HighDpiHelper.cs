// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Forms.HighDpiHelper
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

namespace i_Tree_Eco_v6.Forms
{
  public static class HighDpiHelper
  {
    public static void AdjustControlImagesDpiScale(Control container)
    {
      float dpiScale = HighDpiHelper.GetDpiScale(container).Value;
      if (HighDpiHelper.CloseToOne(dpiScale))
        return;
      HighDpiHelper.AdjustControlImagesDpiScale(container.Controls, dpiScale);
    }

    private static void AdjustButtonImageDpiScale(ButtonBase button, float dpiScale)
    {
      Image image = button.Image;
      if (image == null)
        return;
      button.Image = HighDpiHelper.ScaleImage(image, dpiScale);
    }

    private static void AdjustControlImagesDpiScale(
      Control.ControlCollection controls,
      float dpiScale)
    {
      foreach (Control control in (ArrangedElementCollection) controls)
      {
        switch (control)
        {
          case ButtonBase button:
            HighDpiHelper.AdjustButtonImageDpiScale(button, dpiScale);
            break;
          case PictureBox pictureBox:
            HighDpiHelper.AdjustPictureBoxDpiScale(pictureBox, dpiScale);
            break;
        }
        HighDpiHelper.AdjustControlImagesDpiScale(control.Controls, dpiScale);
      }
    }

    private static void AdjustPictureBoxDpiScale(PictureBox pictureBox, float dpiScale)
    {
      if (pictureBox.Image == null || pictureBox.SizeMode != PictureBoxSizeMode.CenterImage && pictureBox.SizeMode != PictureBoxSizeMode.AutoSize)
        return;
      pictureBox.Image = HighDpiHelper.ScaleImage(pictureBox.Image, dpiScale);
    }

    private static bool CloseToOne(float dpiScale) => (double) Math.Abs(dpiScale - 1f) < 0.001;

    private static Lazy<float> GetDpiScale(Control control) => new Lazy<float>((Func<float>) (() =>
    {
      using (Graphics graphics = control.CreateGraphics())
        return graphics.DpiX / 96f;
    }));

    private static Image ScaleImage(Image image, float dpiScale)
    {
      Size size = HighDpiHelper.ScaleSize(image.Size, dpiScale);
      Bitmap bitmap = new Bitmap(size.Width, size.Height);
      using (Graphics graphics = Graphics.FromImage((Image) bitmap))
      {
        InterpolationMode interpolationMode = InterpolationMode.HighQualityBicubic;
        if ((double) dpiScale >= 2.0)
          interpolationMode = InterpolationMode.NearestNeighbor;
        graphics.InterpolationMode = interpolationMode;
        graphics.DrawImage(image, new Rectangle(new Point(), size));
      }
      return (Image) bitmap;
    }

    private static Size ScaleSize(Size size, float scale) => new Size((int) ((double) size.Width * (double) scale), (int) ((double) size.Height * (double) scale));
  }
}
