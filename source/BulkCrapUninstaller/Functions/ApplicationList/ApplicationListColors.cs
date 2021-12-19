/*
    Copyright (c) 2018 Marcin Szeniak (https://github.com/Klocman/)
    Apache License Version 2.0
*/

using System.Drawing;

namespace BulkCrapUninstaller.Functions.ApplicationList
{
    internal class ApplicationListColors
    {   
        //Light
        public static ApplicationListColors NormalLight = new ApplicationListColors(
            Color.FromArgb(unchecked((int) 0xffccffcc)), //verified
            Color.FromArgb(unchecked((int) 0xffbbddff)), //unverified
            Color.FromArgb(unchecked((int) 0xffE0E0E0)), //invalid
            Color.FromArgb(unchecked((int) 0xffffdbcd)), //unregistered
            Color.FromArgb(unchecked((int) 0xffe7cfff)), //windowsFeature
            Color.FromArgb(unchecked((int) 0xffa3ffff))  //windowsApp
        );

        public static ApplicationListColors ColorBlindLight = new ApplicationListColors(
            Color.FromArgb(unchecked((int) 0xfff6382d)),
            Color.FromArgb(unchecked((int) 0xfffc8d59)),
            Color.FromArgb(unchecked((int) 0xff5189d3)),
            Color.FromArgb(unchecked((int) 0xff91bfdb)),
            Color.FromArgb(unchecked((int) 0xfffee090)),
            Color.FromArgb(unchecked((int) 0xffc9dade))
        );

        //Dark
        public static ApplicationListColors NormalDark = new ApplicationListColors(
            Color.FromArgb(unchecked((int) 0xff59FF59)),
            Color.FromArgb(unchecked((int) 0xff59ACFF)),
            Color.FromArgb(unchecked((int) 0xffDD4D4D)),
            Color.FromArgb(unchecked((int) 0xffFF8559)),
            Color.FromArgb(unchecked((int) 0xffAC59FF)),
            Color.FromArgb(unchecked((int) 0xff6A00FF))
        );

        public static ApplicationListColors ColorBlindDark = new ApplicationListColors(
            Color.FromArgb(unchecked((int) 0xfff6382d)),
            Color.FromArgb(unchecked((int) 0xfffc8d59)),
            Color.FromArgb(unchecked((int) 0xff5189d3)),
            Color.FromArgb(unchecked((int) 0xff91bfdb)),
            Color.FromArgb(unchecked((int) 0xfffee090)),
            Color.FromArgb(unchecked((int) 0xffc9dade))
        );

        public ApplicationListColors(Color verifiedColor, Color unverifiedColor, Color invalidColor,
            Color unregisteredColor, Color windowsFeatureColor, Color windowsStoreAppColor)
        {
            VerifiedColor = verifiedColor;
            UnverifiedColor = unverifiedColor;
            InvalidColor = invalidColor;
            UnregisteredColor = unregisteredColor;
            WindowsFeatureColor = windowsFeatureColor;
            WindowsStoreAppColor = windowsStoreAppColor;
        }

        public Color VerifiedColor { get; }
        public Color UnverifiedColor { get; }
        public Color InvalidColor { get; }
        public Color UnregisteredColor { get; }
        public Color WindowsFeatureColor { get; }
        public Color WindowsStoreAppColor { get; }
    }
}