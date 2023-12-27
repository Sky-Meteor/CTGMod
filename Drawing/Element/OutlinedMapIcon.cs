using Microsoft.Xna.Framework.Graphics;
using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent;

namespace CTGMod.Drawing.Element;

public class OutlinedMapIcon : ARenderTargetContentByRequest
{
    protected int width = 84;
    protected int height = 84;
    public Color _borderColor = Color.White;
    private EffectPass _coloringShader;
    private RenderTarget2D _helperTarget;
    private Texture2D _texture;

    public OutlinedMapIcon(Texture2D texture, Color borderColor)
    {
        _borderColor = borderColor;
        _wasPrepared = false;
        _texture = texture;
        width = texture.Width + 8;
        height = texture.Height + 8;
    }

    public void PrepareRenderTarget() => PrepareRenderTarget(Main.instance.GraphicsDevice, Main.spriteBatch);

    protected override void HandleUseReqest(GraphicsDevice device, SpriteBatch spriteBatch)
    {
        Effect pixelShader = Main.pixelShader;
        if (_coloringShader == null)
            _coloringShader = pixelShader.CurrentTechnique.Passes["ColorOnly"];
        Rectangle rectangle = new Rectangle(0, 0, width, height);
        PrepareARenderTarget_AndListenToEvents(ref _target, device, width, height, RenderTargetUsage.PreserveContents);
        PrepareARenderTarget_WithoutListeningToEvents(ref _helperTarget, device, width, height, RenderTargetUsage.DiscardContents);
        device.SetRenderTarget(_helperTarget);
        device.Clear(Color.Transparent);
        spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null);
        DrawTheContent(spriteBatch);
        spriteBatch.End();
        device.SetRenderTarget(_target);
        device.Clear(Color.Transparent);
        spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null);
        _coloringShader.Apply();
        int num1 = 2;
        int num2 = num1 * 2;
        for (int x = -num2; x <= num2; x += num1)
        {
            for (int y = -num2; y <= num2; y += num1)
            {
                if (Math.Abs(x) + Math.Abs(y) == num2)
                    spriteBatch.Draw(_helperTarget, new Vector2(x, y), Color.Black);
            }
        }
        int num3 = num1;
        for (int x = -num3; x <= num3; x += num1)
        {
            for (int y = -num3; y <= num3; y += num1)
            {
                if (Math.Abs(x) + Math.Abs(y) == num3)
                    spriteBatch.Draw(_helperTarget, new Vector2(x, y), _borderColor);
            }
        }
        pixelShader.CurrentTechnique.Passes[0].Apply();
        spriteBatch.Draw(_helperTarget, Vector2.Zero, Color.White);
        spriteBatch.End();
        device.SetRenderTarget(null);
        _wasPrepared = true;
    }

    internal void DrawTheContent(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(_texture, new Vector2(4f, 4f), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
    }
}