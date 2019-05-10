using System;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace MahApps.Metro.Controls
{
	[ContentProperty("Child")]
	public class Planerator : FrameworkElement
	{
		public readonly static DependencyProperty RotationXProperty;

		public readonly static DependencyProperty RotationYProperty;

		public readonly static DependencyProperty RotationZProperty;

		public readonly static DependencyProperty FieldOfViewProperty;

		private readonly static Point3D[] Mesh;

		private readonly static Point[] TexCoords;

		private readonly static int[] Indices;

		private readonly static Vector3D XAxis;

		private readonly static Vector3D YAxis;

		private readonly static Vector3D ZAxis;

		private readonly QuaternionRotation3D _quaternionRotation;

		private readonly RotateTransform3D _rotationTransform;

		private readonly ScaleTransform3D _scaleTransform;

		private FrameworkElement _logicalChild;

		private FrameworkElement _originalChild;

		private Viewport3D _viewport3D;

		private FrameworkElement _visualChild;

		private Viewport2DVisual3D _frontModel;

		public FrameworkElement Child
		{
			get
			{
				return this._originalChild;
			}
			set
			{
				if (this._originalChild == value)
				{
					return;
				}
				base.RemoveVisualChild(this._visualChild);
				base.RemoveLogicalChild(this._logicalChild);
				this._originalChild = value;
				this._logicalChild = new LayoutInvalidationCatcher()
				{
					Child = this._originalChild
				};
				this._visualChild = this.CreateVisualChild();
				base.AddVisualChild(this._visualChild);
				base.AddLogicalChild(this._logicalChild);
				base.InvalidateMeasure();
			}
		}

		public double FieldOfView
		{
			get
			{
				return (double)base.GetValue(Planerator.FieldOfViewProperty);
			}
			set
			{
				base.SetValue(Planerator.FieldOfViewProperty, value);
			}
		}

		public double RotationX
		{
			get
			{
				return (double)base.GetValue(Planerator.RotationXProperty);
			}
			set
			{
				base.SetValue(Planerator.RotationXProperty, value);
			}
		}

		public double RotationY
		{
			get
			{
				return (double)base.GetValue(Planerator.RotationYProperty);
			}
			set
			{
				base.SetValue(Planerator.RotationYProperty, value);
			}
		}

		public double RotationZ
		{
			get
			{
				return (double)base.GetValue(Planerator.RotationZProperty);
			}
			set
			{
				base.SetValue(Planerator.RotationZProperty, value);
			}
		}

		protected override int VisualChildrenCount
		{
			get
			{
				if (this._visualChild != null)
				{
					return 1;
				}
				return 0;
			}
		}

		static Planerator()
		{
			Class6.yDnXvgqzyB5jw();
			Planerator.RotationXProperty = DependencyProperty.Register("RotationX", typeof(double), typeof(Planerator), new UIPropertyMetadata((object)0, (DependencyObject d, DependencyPropertyChangedEventArgs args) => ((Planerator)d).UpdateRotation()));
			Planerator.RotationYProperty = DependencyProperty.Register("RotationY", typeof(double), typeof(Planerator), new UIPropertyMetadata((object)0, (DependencyObject d, DependencyPropertyChangedEventArgs args) => ((Planerator)d).UpdateRotation()));
			Planerator.RotationZProperty = DependencyProperty.Register("RotationZ", typeof(double), typeof(Planerator), new UIPropertyMetadata((object)0, (DependencyObject d, DependencyPropertyChangedEventArgs args) => ((Planerator)d).UpdateRotation()));
			Planerator.FieldOfViewProperty = DependencyProperty.Register("FieldOfView", typeof(double), typeof(Planerator), new UIPropertyMetadata((object)45, (DependencyObject d, DependencyPropertyChangedEventArgs args) => ((Planerator)d).Update3D(), (DependencyObject d, object val) => Math.Min(Math.Max((double)val, 0.5), 179.9)));
			Planerator.Mesh = new Point3D[] { new Point3D(0, 0, 0), new Point3D(0, 1, 0), new Point3D(1, 1, 0), new Point3D(1, 0, 0) };
			Planerator.TexCoords = new Point[] { new Point(0, 1), new Point(0, 0), new Point(1, 0), new Point(1, 1) };
			Planerator.Indices = new int[] { 0, 2, 1, 0, 3, 2 };
			Planerator.XAxis = new Vector3D(1, 0, 0);
			Planerator.YAxis = new Vector3D(0, 1, 0);
			Planerator.ZAxis = new Vector3D(0, 0, 1);
		}

		public Planerator()
		{
			Class6.yDnXvgqzyB5jw();
			this._quaternionRotation = new QuaternionRotation3D();
			this._rotationTransform = new RotateTransform3D();
			this._scaleTransform = new ScaleTransform3D();
			base();
		}

		protected override Size ArrangeOverride(Size finalSize)
		{
			if (this._logicalChild != null)
			{
				this._logicalChild.Arrange(new Rect(finalSize));
				this._visualChild.Arrange(new Rect(finalSize));
				this.Update3D();
			}
			return base.ArrangeOverride(finalSize);
		}

		private FrameworkElement CreateVisualChild()
		{
			MeshGeometry3D meshGeometry3D = new MeshGeometry3D()
			{
				Positions = new Point3DCollection(Planerator.Mesh),
				TextureCoordinates = new PointCollection(Planerator.TexCoords),
				TriangleIndices = new Int32Collection(Planerator.Indices)
			};
			Material diffuseMaterial = new DiffuseMaterial(Brushes.White);
			diffuseMaterial.SetValue(Viewport2DVisual3D.IsVisualHostMaterialProperty, (object)true);
			VisualBrush visualBrush = new VisualBrush(this._logicalChild);
			this.SetCachingForObject(visualBrush);
			Material material = new DiffuseMaterial(visualBrush);
			this._rotationTransform.Rotation = this._quaternionRotation;
			Transform3DGroup transform3DGroup = new Transform3DGroup();
			transform3DGroup.Children.Add(this._scaleTransform);
			transform3DGroup.Children.Add(this._rotationTransform);
			Transform3DGroup transform3DGroup1 = transform3DGroup;
			GeometryModel3D geometryModel3D = new GeometryModel3D()
			{
				Geometry = meshGeometry3D,
				Transform = transform3DGroup1,
				BackMaterial = material
			};
			Model3DGroup model3DGroup = new Model3DGroup();
			model3DGroup.Children.Add(new DirectionalLight(Colors.White, new Vector3D(0, 0, -1)));
			model3DGroup.Children.Add(new DirectionalLight(Colors.White, new Vector3D(0.1, -0.1, 1)));
			model3DGroup.Children.Add(geometryModel3D);
			ModelVisual3D modelVisual3D = new ModelVisual3D()
			{
				Content = model3DGroup
			};
			if (this._frontModel != null)
			{
				this._frontModel.Visual = null;
			}
			this._frontModel = new Viewport2DVisual3D()
			{
				Geometry = meshGeometry3D,
				Visual = this._logicalChild,
				Material = diffuseMaterial,
				Transform = transform3DGroup1
			};
			this.SetCachingForObject(this._frontModel);
			Viewport3D viewport3D = new Viewport3D()
			{
				ClipToBounds = false
			};
			viewport3D.Children.Add(modelVisual3D);
			viewport3D.Children.Add(this._frontModel);
			this._viewport3D = viewport3D;
			this.UpdateRotation();
			return this._viewport3D;
		}

		protected override Visual GetVisualChild(int index)
		{
			return this._visualChild;
		}

		protected override Size MeasureOverride(Size availableSize)
		{
			Size size;
			if (this._logicalChild == null)
			{
				size = new Size(0, 0);
			}
			else
			{
				this._logicalChild.Measure(availableSize);
				size = this._logicalChild.DesiredSize;
				this._visualChild.Measure(size);
			}
			return size;
		}

		private void SetCachingForObject(DependencyObject d)
		{
			RenderOptions.SetCachingHint(d, CachingHint.Cache);
			RenderOptions.SetCacheInvalidationThresholdMinimum(d, 0.5);
			RenderOptions.SetCacheInvalidationThresholdMaximum(d, 2);
		}

		private void Update3D()
		{
			Rect descendantBounds = VisualTreeHelper.GetDescendantBounds(this._logicalChild);
			double width = descendantBounds.Width;
			double height = descendantBounds.Height;
			double fieldOfView = this.FieldOfView * 0.0174532925199433;
			double num = width / Math.Tan(fieldOfView / 2) / 2;
			this._viewport3D.Camera = new PerspectiveCamera(new Point3D(width / 2, height / 2, num), -Planerator.ZAxis, Planerator.YAxis, this.FieldOfView);
			this._scaleTransform.ScaleX = width;
			this._scaleTransform.ScaleY = height;
			this._rotationTransform.CenterX = width / 2;
			this._rotationTransform.CenterY = height / 2;
		}

		private void UpdateRotation()
		{
			Quaternion quaternion = new Quaternion(Planerator.XAxis, this.RotationX);
			Quaternion quaternion1 = new Quaternion(Planerator.YAxis, this.RotationY);
			Quaternion quaternion2 = new Quaternion(Planerator.ZAxis, this.RotationZ);
			this._quaternionRotation.Quaternion = (quaternion * quaternion1) * quaternion2;
		}
	}
}