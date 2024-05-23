using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace Everglow.Commons.Skeleton2D.Renderer.DrawCommands;

public class ImmediateExecuter : IDrawCommandExecuter, IDrawCommandVisitor
{
	private GraphicsDevice graphicsDevice;

	public void Visit<T>(DrawMesh<T> command) where T : struct, IVertexType
	{
		graphicsDevice.Textures[0] = command.PipelineStateObject.Texture;
		this.graphicsDevice.DrawUserPrimitives<T>(command.PrimitiveType, command.Vertices.ToArray(), command.Offset, command.GeometryCount);
	}

	public void Visit<T>(DrawIndexedMesh<T> command) where T : struct, IVertexType
	{
		graphicsDevice.Textures[0] = command.PipelineStateObject.Texture;
		this.graphicsDevice.DrawUserIndexedPrimitives<T>(command.PrimitiveType, command.Vertices.ToArray(), command.VertexStart, command.VertexCount, 
			command.Indices.ToArray(), command.IndexStart, command.GeometryCount);
	}

	public void Execute(DrawCommandList commandList, GraphicsDevice graphicsDevice)
	{
		this.graphicsDevice = graphicsDevice;
		foreach (DrawCommand command in commandList)
		{
			command.Accept(this);
		}
	}
}