using System;

namespace _2DEngine
{

	public struct Matrix4x4
	{
		public Vector4 column_0;
		public Vector4 column_1;
		public Vector4 column_2;
		public Vector4 column_3;

		public static readonly Matrix4x4 identity = new Matrix4x4(new Vector4(1f, 0f, 0f, 0f), new Vector4(0f, 1f, 0f, 0f), new Vector4(0f, 0f, 1f, 0f), new Vector4(0f, 0f, 0f, 1f));
		public Vector4 GetLine (int index)
		{
			switch (index)
			{
				case 0: return new Vector4(column_0.x, column_1.x, column_2.x, column_3.x);
				case 1: return new Vector4(column_0.y, column_1.y, column_2.y, column_3.y);
				case 2: return new Vector4(column_0.z, column_1.z, column_2.z, column_3.z);
				case 3: return new Vector4(column_0.w, column_1.w, column_2.w, column_3.w);
				default: return new Vector4();
			}
		}

		public Matrix4x4 (
			float m11, float m12, float m13, float m14,
			float m21, float m22, float m23, float m24,
			float m31, float m32, float m33, float m34,
			float m41, float m42, float m43, float m44) : this (
				new Vector4(m11, m21, m31, m41),
				new Vector4(m12, m22, m32, m42),
				new Vector4(m13, m23, m33, m43),
				new Vector4(m14, m24, m34, m44))
		{
		}

		public static Matrix4x4 CreateViewport (int width, int height)
		{
			Matrix4x4 m = new Matrix4x4();
			m.column_0 = new Vector4(width / 2, 0f, 0f, 0);
			m.column_1 = new Vector4(0f, height / 2, 0f, 0);
			m.column_2 = new Vector4(0f, 0f, 1f, 0f);
			m.column_3 = new Vector4(0f, 0f, 0f, 1f);

			return m;
		}

		public Matrix4x4 (Vector4 column_0, Vector4 column_1, Vector4 column_2, Vector4 column_3)
		{
			this.column_0 = column_0;
			this.column_1 = column_1;
			this.column_2 = column_2;
			this.column_3 = column_3;
		}

		public static Vector4 operator * (Matrix4x4 m, Vector4 v)
		{
			float x = Vector4.Dot (v, m.GetLine(0));
			float y = Vector4.Dot (v, m.GetLine(1));
			float z = Vector4.Dot (v, m.GetLine(2));
			float w = Vector4.Dot (v, m.GetLine(3));
			return new Vector4 (x, y, z, w);
		}
		public static Matrix4x4 operator * (Matrix4x4 a, Matrix4x4 b)
		{
			Vector4 c0 = b * a.column_0;
			Vector4 c1 = b * a.column_1;
			Vector4 c2 = b * a.column_2;
			Vector4 c3 = b * a.column_3;

			return new Matrix4x4(c0, c1, c2, c3);
		}

		public static Matrix4x4 operator * (Matrix4x4 matrix, float f)
		{
			matrix.column_0 *= f;
			matrix.column_1 *= f;
			matrix.column_2 *= f;
			matrix.column_3 *= f;
			return matrix;
		}

		public float this[int i, int j]
		{
			get
			{
				switch (j)
				{
					case 0:return column_0[i];
					case 1:return column_1[i];
					case 2:return column_2[i];
					case 3:return column_3[i];
					default:return 0;
				}
			}
			set
			{
				switch (j)
				{
					case 0: column_0[i] = value; break;
					case 1: column_1[i] = value; break;
					case 2: column_2[i] = value; break;
					case 3: column_3[i] = value; break;
				}
			}
		}

		public float GetDeterminant ()
		{
			float SubFactor00 = this[2, 2] * this[3, 3] - this[3, 2] * this[2, 3];
			float SubFactor01 = this[2, 1] * this[3, 3] - this[3, 1] * this[2, 3];
			float SubFactor02 = this[2, 1] * this[3, 2] - this[3, 1] * this[2, 2];
			float SubFactor03 = this[2, 0] * this[3, 3] - this[3, 0] * this[2, 3];
			float SubFactor04 = this[2, 0] * this[3, 2] - this[3, 0] * this[2, 2];
			float SubFactor05 = this[2, 0] * this[3, 1] - this[3, 0] * this[2, 1];

			Vector4 DetCof = new Vector4(
				+(this[1, 1] * SubFactor00 - this[1, 2] * SubFactor01 + this[1, 3] * SubFactor02),
				-(this[1, 0] * SubFactor00 - this[1, 2] * SubFactor03 + this[1, 3] * SubFactor04),
				+(this[1, 0] * SubFactor01 - this[1, 1] * SubFactor03 + this[1, 3] * SubFactor05),
				-(this[1, 0] * SubFactor02 - this[1, 1] * SubFactor04 + this[1, 2] * SubFactor05)
			);

			return
				this[0, 0] * DetCof[0] + this[0, 1] * DetCof[1] +
				this[0, 2] * DetCof[2] + this[0, 3] * DetCof[3];
		}

		public Matrix4x4 GetTransponed ()
		{
			return new Matrix4x4(GetLine(0), GetLine(1), GetLine(2), GetLine(3));
		}
		public Matrix4x4 GetInversed ()
		{
			return GetTransponed().GetAdj () * (1f / GetDeterminant ());
		}

		public Matrix4x4 GetAdj()
		{
			Matrix4x4 resoult = new Matrix4x4();
			for (int i_x = 0; i_x < 4; i_x++)
			{
				for (int i_y = 0; i_y < 4; i_y++)
				{
					resoult[i_x, i_y] = (-1f).Pow(i_x + i_y) * MinorStage(i_x, i_y);
				}
			}
			return resoult;
		}
		public Matrix3x3 Minor(int x, int y)
		{
			var m = new Matrix3x3();
			int nx = 0;

			for (int i = 0; i < 4; i++)
			{
				int ny = 0;
				if (i == x) continue;
				for (int j = 0; j < 4; j++)
				{
					if (j == y) continue;
					m[nx, ny] = this[i, j];
					ny++;
				}
				nx++;
			}

			return m;
		}
		private float MinorStage(int x, int y)
		{
			return Minor(x, y).GetDeterminant();
		}

		public override string ToString ()
		{
			return string.Format ("{0}\n{1}\n{2}\n{3}", column_0.ToString (), column_1.ToString (), column_2.ToString (), column_3.ToString());
		}

		public Matrix4x4 Transpose (Vector3 position)
		{
			return new Matrix4x4 (column_0, column_1, column_2, new Vector4 (position.x, position.y, position.z, 1f));
		}
		public Matrix4x4 Translate (Vector3 position)
		{
			Matrix4x4 matrix = identity;
			matrix.column_3 = new Vector4(position.x, position.y, position.z, 1f);
			return matrix * this;
		}
		public static Matrix4x4 CreateWorldMatrix (Vector3 right, Vector3 up, Vector3 forward, Vector3 position)
		{
			return new Matrix4x4 (right, up, forward, new Vector4 (position.x, position.y, position.z, 1f));
		}
		public static Matrix4x4 CreateWorldMatrix(Vector3 position, Vector3 size)
		{
			return new Matrix4x4(new Vector3(size.x, 0f, 0f), new Vector3(0f, size.y, 0f), new Vector3(0f, 0f, size.z), new Vector4(position.x, position.y, position.z, 1f));
		}
		public static Matrix4x4 CreateScaleMatrix(Vector3 scale)
		{
			return new Matrix4x4(new Vector3(scale.x, 0f, 0f), new Vector3(0f, scale.y, 0f), new Vector3(0f, 0f, scale.z), new Vector4(0f, 0f, 0f, 1f));
		}
		public Matrix4x4 Rotate(Vector3 euler)
		{
			return this * CreateRotationMatrix(euler);
		}
		public static Matrix4x4 RotateAround(Vector3 euler, Vector3 position)
		{
			return CreateRotationMatrix(euler) * CreateTranslationMatrix(position);
		}
		public static Matrix4x4 CreateRotationMatrix(Vector3 euler)
		{
			return CreateRotationMatrix_Z(euler.z) * CreateRotationMatrix_Y(euler.y) * CreateRotationMatrix_X(euler.x);
		}
		public static Matrix4x4 CreateTranslationMatrix(Vector3 pos)
		{
			return new Matrix4x4(Vector3.right, Vector3.up, Vector3.forward, new Vector4(pos.x, pos.y, pos.z, 1f));
		}

		public static Matrix4x4 Lerp(Matrix4x4 a, Matrix4x4 b, float t)
		{
			a.column_0 = Vector4.Lerp(a.column_0, b.column_0, t);
			a.column_1 = Vector4.Lerp(a.column_1, b.column_1, t);
			a.column_2 = Vector4.Lerp(a.column_2, b.column_2, t);
			a.column_3 = Vector4.Lerp(a.column_3, b.column_3, t);
			return a;
		}
		public static Matrix4x4 CreateRotationMatrix_X(float angle)
		{
			float sin = angle.Sin();
			float cos = angle.Cos();

			Vector4 c_0 = new Vector4(1, 0, 0, 0);
			Vector4 c_1 = new Vector4(0, cos, sin, 0);
			Vector4 c_2 = new Vector4(0, -sin, cos, 0);

			Vector4 c_3 = new Vector4(0, 0, 0, 1);
			return new Matrix4x4(c_0, c_1, c_2, c_3);
		}
		public static Matrix4x4 CreateRotationMatrix_Y(float angle)
		{
			float sin = angle.Sin();
			float cos = angle.Cos();

			Vector4 c_0 = new Vector4(cos, 0, -sin, 0);
			Vector4 c_1 = new Vector4(0, 1, 0, 0);
			Vector4 c_2 = new Vector4(sin, 0, cos, 0);

			Vector4 c_3 = new Vector4(0, 0, 0, 1);
			return new Matrix4x4(c_0, c_1, c_2, c_3);
		}
		public static Matrix4x4 CreateRotationMatrix_Z(float angle)
		{
			float sin = angle.Sin();
			float cos = angle.Cos();

			Vector4 c_0 = new Vector4(cos, sin, 0, 0);
			Vector4 c_1 = new Vector4(-sin, cos, 0, 0);
			Vector4 c_2 = new Vector4(0, 0, 1, 0);

			Vector4 c_3 = new Vector4(0, 0, 0, 1);
			return new Matrix4x4(c_0, c_1, c_2, c_3);
		}
		public static Matrix4x4 LookRotation (Vector3 to, Vector3 up)
		{
			Vector3 fwd = to;
			Vector3 right = Vector3.Cross(up, to);
			up = Vector3.Cross(fwd, right);
			return new Matrix4x4(right, up, fwd, new Vector4(0,0,0,1));
		}


		public static Matrix4x4 CreateFrustumMatrix(float fov, float aspect, float near, float far)
		{
			float tan = Mathf.Tan(fov * 0.5f);
			Vector4 row1 = new Vector4(1f / (aspect * tan), 0f, 0f, 0f);
			Vector4 row2 = new Vector4(0f, 1f / tan, 0f, 0f);
			Vector4 row3 = new Vector4(0f, 0f, 0f, 1f);
			Vector4 row4 = new Vector4(0f, 0f, -1f, 0f);

			return new Matrix4x4(row1, row2, row3, row4);
		}
		public static Matrix4x4 CreateOrthoMatrix(Vector2 screenSize)
		{
			Vector4 row1 = new Vector4(0f, 0f, 0f, 0f);
			Vector4 row2 = new Vector4(0f, 0f, 0f, 0f);
			Vector4 row3 = new Vector4(0f, 0f, 0f, 0f);
			Vector4 row4 = new Vector4(0f, 0f, 0f, 0f);

			return new Matrix4x4(row1, row2, row3, row4).GetTransponed();
		}

		public Vector3 MultiplyPoint(Vector3 point)
		{
			return (Vector3)(this * new Vector4(point.x, point.y, point.z, 1f));
		}
		public Vector2 MultiplyPoint(Vector2 point)
		{
			return (Vector2)(this * new Vector4(point.x, point.y, 0f, 1f));
		}
		public Vector3 MultiplyVector(Vector3 point)
		{
			return (Vector3)(this * new Vector4(point.x, point.y, point.z, 0f));
		}
		public Vector2 MultiplyVector(Vector2 point)
		{
			return (Vector2)(this * new Vector4(point.x, point.y, 0f, 0f));
		}
		public Vector3 MultiplyDirection(Vector3 direction)
		{
			return (Vector3)(WithoutScale() * new Vector4(direction.x, direction.y, direction.z, 0f));
		}
		public Vector2 MultiplyDirection(Vector2 direction)
		{
			return (Vector2)(WithoutScale() * new Vector4(direction.x, direction.y, 0f, 0f));
		}

		public Matrix4x4 WithoutScale()
		{
			Matrix4x4 m = this;
			m.column_0 = ((Vector3)column_0).normalized;
			m.column_1 = ((Vector3)column_1).normalized;
			m.column_2 = ((Vector3)column_2).normalized;
			return m;
		}

		public Vector3 MultiplyPoint_With_WDevision(Vector3 point)
		{
			return (this * new Vector4(point.x, point.y, point.z, 1f)).ToVector3WithWDevision();
		}


		public Vector3 MultiplySize(Vector3 point)
		{
			Vector3 size = new Vector3(column_0.length, column_1.length, column_2.length);
			return new Vector3(point.x * size.x, point.y * size.y, point.z * size.z);
		}
	}
}