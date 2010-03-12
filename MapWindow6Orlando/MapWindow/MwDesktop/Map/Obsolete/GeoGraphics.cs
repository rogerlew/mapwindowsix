//********************************************************************************************************
// Product Name: MapWindow.dll Alpha
// Description:  The basic module for MapWindow version 6.0
//********************************************************************************************************
// The contents of this file are subject to the Mozilla Public License Version 1.1 (the "License"); 
// you may not use this file except in compliance with the License. You may obtain a copy of the License at 
// http://www.mozilla.org/MPL/ 
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF 
// ANY KIND, either expressed or implied. See the License for the specificlanguage governing rights and 
// limitations under the License. 
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 7/30/2008 12:40:38 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System.Drawing;
using MapWindow.Geometries;
using MapWindow.Drawing;
namespace MapWindow.GeoMap
{

    /// <summary>
    /// GeoGraphics
    /// </summary>
    public class GeoGraphics
    {
        #region Private Variables

        private Graphics _device;
        private int _stage;
        private int _chunk;
        IEnvelope _envelope;
        IGeoMapFrame _mapFrame;
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of GeoGraphics
        /// </summary>
        public GeoGraphics(Graphics inGraphics, IGeoMapFrame inMapFrame, Rectangle clipRectangle)
        {
            _chunk = inMapFrame.CurrentChunk;
            _device = inGraphics;
            //_envelope = inMapFrame.PixelToProj(clipRectangle);
            _envelope = inMapFrame.BufferToProj(clipRectangle);
            _mapFrame = inMapFrame;
        }

        #endregion

        #region Methods


        /// <summary>
        /// Draws the image so that width and height are calculated in coordinates
        /// </summary>
        /// <param name="img"></param>
        /// <param name="center"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public virtual void DrawImage(Image img, ICoordinate center, double width, double height)
        {
            Envelope env = new Envelope(center.X - width / 2, center.X + width / 2, center.Y - height / 2, center.Y + height / 2);
            Rectangle r = _mapFrame.ProjToBuffer(env);
            _device.DrawImage(img, r);
        }

        /// <summary>
        /// Draws the image given the specified pixel size and central point
        /// </summary>
        /// <param name="img"></param>
        /// <param name="center"></param>
        /// <param name="size"></param>
        public virtual void DrawImage(Image img, ICoordinate center, SizeF size)
        {
            System.Drawing.Point c = _mapFrame.ProjToBuffer(center);
            RectangleF r = new RectangleF((float)c.X - size.Width / 2, (float)c.Y - size.Height / 2, size.Width, size.Height);
            _device.DrawImage(img, r);
        }

        /// <summary>
        /// Draws the specified character at the specified geographic coordinate, adjusted to the specified size.
        /// </summary>
        /// <param name="character"></param>
        /// <param name="center"></param>
        /// <param name="size"></param>
        public virtual void DrawCharacter(ICharacterSymbol character, ICoordinate center, SizeF size)
        {
            System.Drawing.Point c = _mapFrame.ProjToBuffer(center);
            Font tempFont = new Font(character.FontFamilyName, size.Height, character.Style, GraphicsUnit.Pixel);
            Brush brush = new SolidBrush(character.Color);
            _device.TextContrast = 0;
            _device.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            _device.DrawString(character.ToString(), tempFont, brush, c.X - size.Width / 2, c.Y - size.Height / 2);
            brush.Dispose();
            tempFont.Dispose();
        }

        /// <summary>
        /// Draws the character when scaled to geographic sizes.
        /// </summary>
        /// <param name="character"></param>
        /// <param name="center"></param>
        /// <param name="size"></param>
        public virtual void DrawCharacter(ICharacterSymbol character, ICoordinate center, ISize size)
        {
            
            Envelope env = new Envelope(center.X - size.XSize / 2, center.X + size.XSize / 2, center.Y - size.YSize / 2, center.Y + size.YSize / 2);
            Rectangle r = ProjToBuffer(env);
            Font tempFont = new Font(character.FontFamilyName, r.Height, character.Style, GraphicsUnit.Pixel);
            Brush brush = new SolidBrush(character.Color);
            _device.TextContrast = 0;
            _device.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            _device.DrawString(character.ToString(), tempFont, brush, r.Left, r.Top);
            brush.Dispose();
            tempFont.Dispose();
        }

        /// <summary>
        /// Fills a rectangle based on the specified location, pixel size, and color
        /// </summary>
        /// <param name="center"></param>
        /// <param name="size"></param>
        /// <param name="color"></param>
        public virtual void FillRectangle(ICoordinate center, SizeF size, Color color)
        {
            System.Drawing.Point c = _mapFrame.ProjToBuffer(center);
            RectangleF r = new RectangleF((float)c.X - size.Width / 2, (float)c.Y - size.Height / 2, size.Width, size.Height);
            Brush b = new SolidBrush(color);
            _device.FillRectangle(b, r);
            b.Dispose();
        }

        /// <summary>
        /// Converts a single point location into an equivalent geographic coordinate
        /// </summary>
        /// <param name="position">The client coordiante relative to the map control</param>
        /// <returns>The geogrpahic ICoordinate interface</returns>
        public ICoordinate PixelToProj(System.Drawing.Point position)
        {
            return _mapFrame.PixelToProj(position);
        }

        /// <summary>
        /// Converts a rectangle in pixel coordinates relative to the map control into
        /// a geographic envelope.
        /// </summary>
        /// <param name="rect">The rectangle to convert</param>
        /// <returns>An IEnvelope interface</returns>
        public IEnvelope PixelToProj(Rectangle rect)
        {
            return _mapFrame.PixelToProj(rect);
        }

        /// <summary>
        /// Converts a single geographic location into the equivalent point on the 
        /// screen relative to the top left corner of the map.
        /// </summary>
        /// <param name="location">The geographic position to transform</param>
        /// <returns>A System.Drawing.Point with the new location.</returns>
        public System.Drawing.Point ProjToBuffer(ICoordinate location)
        {
            return _mapFrame.ProjToBuffer(location);
        }

        /// <summary>
        /// Converts a single geographic envelope into an equivalent Rectangle
        /// as it would be drawn on the screen.
        /// </summary>
        /// <param name="env">The geographic IEnvelope</param>
        /// <returns>A System.Drawing.Rectangle</returns>
        public Rectangle ProjToBuffer(IEnvelope env)
        {
            return _mapFrame.ProjToBuffer(env);
        }


        /// <summary>
        /// Converts a single geographic location into the equivalent point on the 
        /// screen relative to the top left corner of the map.
        /// </summary>
        /// <param name="location">The geographic position to transform</param>
        /// <returns>A System.Drawing.Point with the new location.</returns>
        public System.Drawing.Point ProjToPixel(ICoordinate location)
        {
            return _mapFrame.ProjToPixel(location);
        }

        /// <summary>
        /// Converts a single geographic envelope into an equivalent Rectangle
        /// as it would be drawn on the screen.
        /// </summary>
        /// <param name="env">The geographic IEnvelope</param>
        /// <returns>A System.Drawing.Rectangle</returns>
        public Rectangle ProjToPixel(IEnvelope env)
        {
            return _mapFrame.ProjToPixel(env);
        }



        

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the geographic domain to draw in the client rectangle
        /// </summary>
        public IEnvelope Envelope
        {
            get { return _envelope; }
            set { _envelope = value; }
        }

        /// <summary>
        /// Gets or sets the stage.  In cases like lines, it is desireable for an entire stage to be drawn
        /// before the next stage.  For example, borders need to be drawn before fill, and selections need
        /// to be drawn after the original content.
        /// </summary>
        public int Stage
        {
            get { return _stage; }
            set { _stage = value; }
        }

        /// <summary>
        /// Gets or sets an integer that represents the number of chunks.  Point layers are especially suited
        /// to being divided into chunks when they are especially numerous.  If the map is moved or changed,
        /// then the chunk will simply be reset.  Otherwise it will cycle to the next chunk.
        /// </summary>
        public int Chunk
        {
            get { return _chunk; }
            set { _chunk = value; }
        }

        /// <summary>
        /// Gets a rectangle that shows the dimensions of the entire map frame in pixels.
        /// </summary>
        public virtual Rectangle ClientRectangle
        {
            get { return _mapFrame.ClientRectangle; }
        }

        /// <summary>
        /// Gets the map frame that drawing is being sent to
        /// </summary>
        public IGeoMapFrame MapFrame
        {
            get { return _mapFrame; }
        }
  

        #endregion



    }
}
