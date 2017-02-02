﻿using System;
using Tao.FreeGlut;
using OpenGL;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace OpenGLTutorial6
{
	class Program
	{
		private static int width = 1280, height = 720;
		private static System.Diagnostics.Stopwatch watch;
		private static NeutrinoGl.MaterialsGl materials_;
		private static NeutrinoGl.RendererGl renderer_;
		private static float time_;
		private static float displace = 200.0f;

		static void Main(string[] args)
		{
			// create an OpenGL window
			Glut.glutInit();
			Glut.glutInitDisplayMode(Glut.GLUT_DOUBLE | Glut.GLUT_DEPTH);
			Glut.glutInitWindowSize(width, height);
			Glut.glutCreateWindow("OpenGL Tutorial");
			
			// provide the Glut callbacks that are necessary for running this tutorial
			Glut.glutIdleFunc(OnRenderFrame);
			Glut.glutDisplayFunc(OnDisplay);
			Glut.glutCloseFunc(OnClose);

			time_ = 0;
			materials_ = new NeutrinoGl.MaterialsGl();
			renderer_ = new NeutrinoGl.RendererGl(materials_, new Neutrino.Effect_A_lot_of_particles(), 
				"..\\..\\effects\\textures\\", Neutrino.NMath.vec3_(displace, 0, 0));

			// load a crate texture
			watch = System.Diagnostics.Stopwatch.StartNew();

			Glut.glutMainLoop();
		}

		private static void OnClose()
		{
			renderer_.shutdown();
			materials_.shutdown();
		}

		private static void OnDisplay()
		{

		}

		private static void OnRenderFrame()
		{
			// calculate how much time has elapsed since the last frame
			watch.Stop();
			float deltaTime = (float)watch.ElapsedTicks / System.Diagnostics.Stopwatch.Frequency;
			watch.Restart();

			Matrix4 modelTranslationMatrix = Matrix4.CreateTranslation(new Vector3(displace, 0, 0));
			Matrix4 modelRotationMatrix = Matrix4.CreateRotationZ(time_);
			Matrix4 modelMatrix = modelTranslationMatrix * modelRotationMatrix;

			time_ += deltaTime;
			renderer_.update(deltaTime, Neutrino.NMath.vec3_(modelMatrix[3].x, modelMatrix[3].y, modelMatrix[3].z));
			
			// set up the OpenGL viewport and clear both the color and depth bits
			Gl.Viewport(0, 0, width, height);
			//Gl.ClearColor(0.5F, 0.5F, 0.5F, 0.0F);
			Gl.ClearColor(0.0F, 0.0F, 0.0F, 0.0F);
			Gl.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

			Matrix4 projMatrix = Matrix4.CreatePerspectiveFieldOfView(60F * (float)Math.PI / 180F, (float)width / height, 1F, 10000F); //.CreateOrthographic(width, height, 1F, 10000F);
			Matrix4 viewMatrix = Matrix4.LookAt(new Vector3(0, 0, 1000), Vector3.Zero, Vector3.Up);
			
			renderer_.render(ref projMatrix, ref viewMatrix, ref modelMatrix);
			
			Glut.glutSwapBuffers();
		}
	}
}
