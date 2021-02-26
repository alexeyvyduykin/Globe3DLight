using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Globe3DLight;
using OpenTK.Graphics.OpenGL;
using GlmSharp;
using Extensions;
using System.Diagnostics;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics;

namespace Globe3DLight.Renderer
{
    public static class Frame
    {
        //public Frame() { }

      //  dvec3 xAxis = dvec3.UnitX;
      //  dvec3 yAxis = dvec3.UnitY;
      //  dvec3 zAxis = dvec3.UnitZ;

        //void glFrameLength(Frame& frame, double length)
        //{
        //    frame.xAxis.normalize();
        //    frame.yAxis.normalize();
        //    frame.zAxis.normalize();
        //    frame.xAxis *= length;
        //    frame.yAxis *= length;
        //    frame.zAxis *= length;
        //}

        static void glDrawAxis()   // zAxis
        {
            GL.Begin(PrimitiveType.Lines);
            GL.Vertex3(0.0, 0.0, 0.0);
            GL.Vertex3(0.0, 0.0, 1.0);
            GL.End();

            //glPushMatrix();
            //glTranslated(0.0, 0.0, 1.0);
            //glutSolidCone(1.0 / 100.0 * 2.0, 1.0 / 7.5, 12, 9);
            //glPopMatrix();
        }

        public static void glDrawFrame(dmat4 mv, dmat4 proj, double length)
        {
      //      ART::Matrix4D mtx4;
      //  mtx4.fromAxes(frame.xAxis, frame.yAxis, frame.zAxis);

            GL.PushMatrix();

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(proj.Values1D);

            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(mv.Values1D);
            
            //GL.MultMatrixd(mtx4[0][0]);

            GL.Scale(length, length, length);

            // zAxis
            GL.Color3(0.0, 0.0, 1.0);
        glDrawAxis();

   //         GL.RasterPos3f(0.0, 0.0, 1.2);
   //     glutBitmapCharacter(GLUT_BITMAP_HELVETICA_12, 'z');


            // xAxis
            GL.Color3(1.0, 0.0, 0.0);
            GL.Rotate(90, 0, 1, 0);
        glDrawAxis();


     //       GL.RasterPos3f(0.0, 0.0, 1.2);
     //   glutBitmapCharacter(GLUT_BITMAP_HELVETICA_12, 'x');


            // yAxis
            GL.Color3(0.0, 1.0, 0.0);
            GL.Rotate(-90, 1, 0, 0);
        glDrawAxis();

     //       GL.RasterPos3f(0.0, 0.0, 1.2);
     //   glutBitmapCharacter(GLUT_BITMAP_HELVETICA_12, 'y');


            GL.PopMatrix();
    }
    }
    /*
    void glDrawFrame1(const Frame& frame, double length)
    {
      glPushMatrix();
        glScalef(length, length, length);

        glBegin(GL_LINES);
          glColor3f(1.0, 0.0, 0.0);
            glVertex3f(0.0, 0.0, 0.0);
            glVertex3f(frame.xAxis.x, frame.xAxis.y, frame.xAxis.z);

          glColor3f(0.0, 1.0, 0.0);
            glVertex3f(0.0, 0.0, 0.0);
            glVertex3f(frame.yAxis.x, frame.yAxis.y, frame.yAxis.z);

          glColor3f(0.0, 0.0, 1.0);
            glVertex3f(0.0, 0.0, 0.0);
            glVertex3f(frame.zAxis.x, frame.zAxis.y, frame.zAxis.z);
        glEnd();


        double thetaX = asin(frame.yAxis.z);
        double thetaY = 0.0f;
        double thetaZ = 0.0f;

        if (thetaX < Math::HALFPI)
        {
            if (thetaX > -Math::HALFPI)
            {
                thetaZ = atan2(-frame.yAxis.x, frame.yAxis.y);
                thetaY = atan2(-frame.xAxis.z, frame.zAxis.z);
            }
            else
            {
                // Not a unique solution.
                thetaZ = -atan2(frame.zAxis.x, frame.xAxis.x);
                thetaY = 0.0f;
            }
        }
        else
        {
            // Not a unique solution.
            thetaZ = atan2(frame.zAxis.x, frame.xAxis.x);
            thetaY = 0.0f;
        }


        glRotatef( thetaZ * ART::Math::RAD_TO_DEG, 0.0, 0.0, 1.0 );
        glRotatef( thetaX * ART::Math::RAD_TO_DEG, 1.0, 0.0, 0.0 );
        glRotatef( thetaY * ART::Math::RAD_TO_DEG, 0.0, 1.0, 0.0 );

        // zAxis
        glColor3f(0.0, 0.0, 1.0);
        glPushMatrix();
          glTranslated(0.0, 0.0, 1.0);
            glutSolidCone(1.0/100.0*2.0, 1.0/7.5, 12, 9);
        glPopMatrix();

        // xAxis
        glColor3f(1.0, 0.0, 0.0);
        glRotated(90, 0, 1, 0);
        glPushMatrix();
          glTranslated(0.0, 0.0, 1.0);
            glutSolidCone(1.0/100.0*2.0, 1.0/7.5, 12, 9);
        glPopMatrix();

        // yAxis
        glColor3f(0.0, 1.0, 0.0);
        glRotated(-90, 1, 0, 0);
        glPushMatrix();
          glTranslated(0.0, 0.0, 1.0);
            glutSolidCone(1.0/100.0*2.0, 1.0/7.5, 12, 9);
        glPopMatrix();

      glPopMatrix();
    }

    void glDrawFrame2(const Frame& frame, double length)
    {
      ART::Matrix4D mtx;
      mtx.fromAxes(frame.xAxis, frame.yAxis, frame.zAxis);

      glPushMatrix();

        glMatrixMode(GL_MODELVIEW);
        glMultMatrixd(&mtx[0][0]);

        glScalef(length, length, length);

        glBegin(GL_LINES);
          glColor3f(1.0, 0.0, 0.0);
            glVertex3f(0.0, 0.0, 0.0);
            glVertex3f(frame.xAxis.x, frame.xAxis.y, frame.xAxis.z);

          glColor3f(0.0, 1.0, 0.0);
            glVertex3f(0.0, 0.0, 0.0);
            glVertex3f(frame.yAxis.x, frame.yAxis.y, frame.yAxis.z);

          glColor3f(0.0, 0.0, 1.0);
            glVertex3f(0.0, 0.0, 0.0);
            glVertex3f(frame.zAxis.x, frame.zAxis.y, frame.zAxis.z);
        glEnd();

        // zAxis
        glColor3f(0.0, 0.0, 1.0);
        glPushMatrix();
          glTranslated(0.0, 0.0, 1.0);
            glutSolidCone(1.0/100.0*2.0, 1.0/7.5, 12, 9);
        glPopMatrix();

        // xAxis
        glColor3f(1.0, 0.0, 0.0);
        glRotated(90, 0, 1, 0);
        glPushMatrix();
          glTranslated(0.0, 0.0, 1.0);
            glutSolidCone(1.0/100.0*2.0, 1.0/7.5, 12, 9);
        glPopMatrix();

        // yAxis
        glColor3f(0.0, 1.0, 0.0);
        glRotated(-90, 1, 0, 0);
        glPushMatrix();
          glTranslated(0.0, 0.0, 1.0);
            glutSolidCone(1.0/100.0*2.0, 1.0/7.5, 12, 9);
        glPopMatrix();

      glPopMatrix();
    }
    */

}

