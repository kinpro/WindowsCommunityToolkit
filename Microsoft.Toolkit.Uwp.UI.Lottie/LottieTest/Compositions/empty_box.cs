//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using Host = Microsoft_UI_Xaml_Controls;
using Microsoft.Graphics.Canvas.Geometry;
using System;
using System.Numerics;
using Windows.UI;
using Windows.UI.Composition;

namespace Compositions
{
    sealed class Empty_box : Host.ICompositionSource
    {
        public bool TryCreateInstance(
            Compositor compositor,
            out Visual rootVisual,
            out Vector2 size,
            out TimeSpan duration,
            out object diagnostics)
        {
            rootVisual = Instantiator.InstantiateComposition(compositor);
            size = new Vector2(120, 120);
            duration = TimeSpan.FromTicks(20000000);
            diagnostics = null;
            return true;
        }

        sealed class Instantiator
        {
            const long c_durationTicks = 20000000;
            readonly Compositor _c;
            readonly ExpressionAnimation _reusableExpressionAnimation;
            CubicBezierEasingFunction _cubicBezierEasingFunction_1;
            CubicBezierEasingFunction _cubicBezierEasingFunction_3;
            LinearEasingFunction _linearEasingFunction;
            ContainerVisual _root;
            ExpressionAnimation _scalarExpressionAnimation;
            StepEasingFunction _stepEasingFunction;

            // Shape layer: "ruoi" in asset: "Root".
            CompositionContainerShape ContainerShape_08()
            {
                var result = _c.CreateContainerShape();
                var propertySet = result.Properties;
                propertySet.InsertVector2("Anchor", new Vector2(60.5309982F, 10.9449997F));
                propertySet.InsertVector2("Position", new Vector2(57.3610001F, 61.0159988F));
                result.CenterPoint = new Vector2(60.5309982F, 10.9449997F);
                var shapes = result.Shapes;
                shapes.Add(ContainerShape_09());
                _reusableExpressionAnimation.ClearAllParameters();
                _reusableExpressionAnimation.Expression = "my.Position - my.Anchor";
                _reusableExpressionAnimation.SetReferenceParameter("my", result);
                result.StartAnimation("Offset", _reusableExpressionAnimation);
                result.StartAnimation("Position", Vector2Animation());
                var controller = result.TryGetAnimationController("Position");
                controller.Pause();
                controller.StartAnimation("Progress", ScalarExpressionAnimation());
                return result;
            }

            // Root.ruoi
            // Group: ruoi
            CompositionContainerShape ContainerShape_09()
            {
                var result = _c.CreateContainerShape();
                result.CenterPoint = new Vector2(60.5309982F, 10.941F);
                var shapes = result.Shapes;
                shapes.Add(ContainerShape_10());
                shapes.Add(ContainerShape_11());
                shapes.Add(ContainerShape_12());
                return result;
            }

            // Root.ruoi
            //   Group: ruoi
            // Group: Group 3
            CompositionContainerShape ContainerShape_10()
            {
                var result = _c.CreateContainerShape();
                result.Offset = new Vector2(57.9570007F, 10.552F);
                var shapes = result.Shapes;
                shapes.Add(SpriteShape_5());
                return result;
            }

            // Root.ruoi
            //   Group: ruoi
            // Group: Group 2
            CompositionContainerShape ContainerShape_11()
            {
                var result = _c.CreateContainerShape();
                result.Offset = new Vector2(64.1449966F, 9.60599995F);
                var shapes = result.Shapes;
                shapes.Add(SpriteShape_6());
                return result;
            }

            // Root.ruoi
            //   Group: ruoi
            // Group: Group 1
            CompositionContainerShape ContainerShape_12()
            {
                var result = _c.CreateContainerShape();
                result.Offset = new Vector2(62.4000015F, 13.1440001F);
                var shapes = result.Shapes;
                shapes.Add(SpriteShape_7());
                return result;
            }

            CubicBezierEasingFunction CubicBezierEasingFunction_2()
            {
                return _c.CreateCubicBezierEasingFunction(new Vector2(0, 0), new Vector2(0.833000004F, 0.833000004F));
            }

            CubicBezierEasingFunction CubicBezierEasingFunction_3()
            {
                return _cubicBezierEasingFunction_3 = _c.CreateCubicBezierEasingFunction(new Vector2(0.166999996F, 0.166999996F), new Vector2(0.833000004F, 0.833000004F));
            }

            CanvasGeometry Geometry_5()
            {
                CanvasGeometry result;
                using (var builder = new CanvasPathBuilder(null))
                {
                    builder.SetFilledRegionDetermination(CanvasFilledRegionDetermination.Winding);
                    builder.BeginFigure(new Vector2(0, -3.61400008F));
                    builder.AddCubicBezier(new Vector2(1.99600005F, -3.61400008F), new Vector2(3.61400008F, -1.99600005F), new Vector2(3.61400008F, 0));
                    builder.AddCubicBezier(new Vector2(3.61400008F, 1.99600005F), new Vector2(1.99600005F, 3.61400008F), new Vector2(0, 3.61400008F));
                    builder.AddCubicBezier(new Vector2(-1.99600005F, 3.61400008F), new Vector2(-3.61400008F, 1.99600005F), new Vector2(-3.61400008F, 0));
                    builder.AddCubicBezier(new Vector2(-3.61400008F, -1.99600005F), new Vector2(-1.99600005F, -3.61400008F), new Vector2(0, -3.61400008F));
                    builder.EndFigure(CanvasFigureLoop.Closed);
                    result = CanvasGeometry.CreatePath(builder);
                }
                return result;
            }

            CanvasGeometry Geometry_6()
            {
                CanvasGeometry result;
                using (var builder = new CanvasPathBuilder(null))
                {
                    builder.SetFilledRegionDetermination(CanvasFilledRegionDetermination.Winding);
                    builder.BeginFigure(new Vector2(0.00100000005F, -2.57399988F));
                    builder.AddCubicBezier(new Vector2(1.42200005F, -2.57399988F), new Vector2(2.57399988F, -1.42200005F), new Vector2(2.57399988F, 0));
                    builder.AddCubicBezier(new Vector2(2.57399988F, 1.42200005F), new Vector2(1.42200005F, 2.57399988F), new Vector2(0.00100000005F, 2.57399988F));
                    builder.AddCubicBezier(new Vector2(-1.421F, 2.57399988F), new Vector2(-2.57399988F, 1.42200005F), new Vector2(-2.57399988F, 0));
                    builder.AddCubicBezier(new Vector2(-2.57399988F, -1.42200005F), new Vector2(-1.421F, -2.57399988F), new Vector2(0.00100000005F, -2.57399988F));
                    builder.EndFigure(CanvasFigureLoop.Closed);
                    result = CanvasGeometry.CreatePath(builder);
                }
                return result;
            }

            CanvasGeometry Geometry_7()
            {
                CanvasGeometry result;
                using (var builder = new CanvasPathBuilder(null))
                {
                    builder.SetFilledRegionDetermination(CanvasFilledRegionDetermination.Winding);
                    builder.BeginFigure(new Vector2(-0.00100000005F, -1.801F));
                    builder.AddCubicBezier(new Vector2(0.994000018F, -1.801F), new Vector2(1.801F, -0.995000005F), new Vector2(1.801F, -0.00100000005F));
                    builder.AddCubicBezier(new Vector2(1.801F, 0.992999971F), new Vector2(0.994000018F, 1.801F), new Vector2(-0.00100000005F, 1.801F));
                    builder.AddCubicBezier(new Vector2(-0.995000005F, 1.801F), new Vector2(-1.801F, 0.992999971F), new Vector2(-1.801F, -0.00100000005F));
                    builder.AddCubicBezier(new Vector2(-1.801F, -0.995000005F), new Vector2(-0.995000005F, -1.801F), new Vector2(-0.00100000005F, -1.801F));
                    builder.EndFigure(CanvasFigureLoop.Closed);
                    result = CanvasGeometry.CreatePath(builder);
                }
                return result;
            }

            LinearEasingFunction LinearEasingFunction()
            {
                return _linearEasingFunction = _c.CreateLinearEasingFunction();
            }


            // Root.ruoi
            //   Group: ruoi
            //     Group: Group 3
            //       Path 1
            // Path 1.PathGeometry
            CompositionPathGeometry PathGeometry_5()
            {
                var result = _c.CreatePathGeometry(new CompositionPath(Geometry_5()));
                return result;
            }

            // Root.ruoi
            //   Group: ruoi
            //     Group: Group 2
            //       Path 1
            // Path 1.PathGeometry
            CompositionPathGeometry PathGeometry_6()
            {
                var result = _c.CreatePathGeometry(new CompositionPath(Geometry_6()));
                return result;
            }

            // Root.ruoi
            //   Group: ruoi
            //     Group: Group 1
            //       Path 1
            // Path 1.PathGeometry
            CompositionPathGeometry PathGeometry_7()
            {
                var result = _c.CreatePathGeometry(new CompositionPath(Geometry_7()));
                return result;
            }

            // The root of the composition.
            ContainerVisual Root()
            {
                var result = _root = _c.CreateContainerVisual();
                var propertySet = result.Properties;
                propertySet.InsertScalar("Progress", 0);
                propertySet.InsertScalar("t0", 0);
                var children = result.Children;
                children.InsertAtTop(ShapeVisual());
                result.StartAnimation("t0", ScalarAnimation_1_to_1());
                var controller = result.TryGetAnimationController("t0");
                controller.Pause();
                controller.StartAnimation("Progress", _scalarExpressionAnimation);
                return result;
            }


            ScalarKeyFrameAnimation ScalarAnimation_1_to_1()
            {
                var result = _c.CreateScalarKeyFrameAnimation();
                result.Duration = TimeSpan.FromTicks(c_durationTicks);
                result.InsertKeyFrame(0.204379901F, 1, CubicBezierEasingFunction_2());
                result.InsertKeyFrame(0.204380006F, 0, _stepEasingFunction);
                result.InsertKeyFrame(0.388899893F, 1, CubicBezierEasingFunction_3());
                result.InsertKeyFrame(0.388899982F, 0, _stepEasingFunction);
                result.InsertKeyFrame(0.699999928F, 1, _cubicBezierEasingFunction_3);
                return result;
            }

            ExpressionAnimation ScalarExpressionAnimation()
            {
                var result = _scalarExpressionAnimation = _c.CreateExpressionAnimation();
                result.SetReferenceParameter("_", _root);
                result.Expression = "_.Progress";
                return result;
            }

            ShapeVisual ShapeVisual()
            {
                var result = _c.CreateShapeVisual();
                result.Size = new Vector2(120, 120);
                var shapes = result.Shapes;
                // Root.ruoi
                shapes.Add(ContainerShape_08());
                return result;
            }

            // Root.ruoi
            //   Group: ruoi
            //     Group: Group 3
            // Path 1
            CompositionSpriteShape SpriteShape_5()
            {
                var result = _c.CreateSpriteShape();
                result.FillBrush = _c.CreateColorBrush(Color.FromArgb(0xff, 0xff, 0, 0));
                result.Geometry = PathGeometry_5();
                result.StrokeBrush = _c.CreateColorBrush(Color.FromArgb(0xff, 0xff, 0xff, 0xff));
                result.StrokeMiterLimit = 10;
                result.StrokeThickness = 0.699999988F;
                return result;
            }

            // Root.ruoi
            //   Group: ruoi
            //     Group: Group 2
            // Path 1
            CompositionSpriteShape SpriteShape_6()
            {
                var result = _c.CreateSpriteShape();
                result.FillBrush = _c.CreateColorBrush(Colors.Yellow);
                result.Geometry = PathGeometry_6();
                result.StrokeBrush = _c.CreateColorBrush(Colors.Green);
                result.StrokeMiterLimit = 10;
                result.StrokeThickness = 0.699999988F;
                return result;
            }

            // Root.ruoi
            //   Group: ruoi
            //     Group: Group 1
            // Path 1
            CompositionSpriteShape SpriteShape_7()
            {
                var result = _c.CreateSpriteShape();
                result.FillBrush = _c.CreateColorBrush(Colors.Blue);
                result.Geometry = PathGeometry_7();
                return result;
            }

            StepEasingFunction StepEasingFunction()
            {
                var result = _stepEasingFunction = _c.CreateStepEasingFunction();
                result.IsInitialStepSingleFrame  = true;
                return result;
            }

            // Root.ruoi
            // Position
            Vector2KeyFrameAnimation Vector2Animation()
            {
                var result = _c.CreateVector2KeyFrameAnimation();
                result.SetReferenceParameter("_", _root);
                result.Duration = TimeSpan.FromTicks(c_durationTicks);
                result.InsertKeyFrame(0, new Vector2(57.3610001F, 61.0159988F), LinearEasingFunction());
                result.InsertExpressionKeyFrame(0.204379901F, "(Pow(1 - _.t0, 3) * Vector2(57.361,61.016)) + (3 * Square(1 - _.t0) * _.t0 * Vector2(52.686,56.88799)) + (3 * (1 - _.t0) * Square(_.t0) * Vector2(43.78901,47.069)) + (Pow(_.t0, 3) * Vector2(57.699,41.796))", StepEasingFunction());
                result.InsertExpressionKeyFrame(0.388899893F, "(Pow(1 - _.t0, 3) * Vector2(57.699,41.796)) + (3 * Square(1 - _.t0) * _.t0 * Vector2(70.515,36.938)) + (3 * (1 - _.t0) * Square(_.t0) * Vector2(74.53902,37.716)) + (Pow(_.t0, 3) * Vector2(79.084,33.982))", _stepEasingFunction);
                result.InsertExpressionKeyFrame(0.699999928F, "(Pow(1 - _.t0, 3) * Vector2(79.084,33.982)) + (3 * Square(1 - _.t0) * _.t0 * Vector2(85.70001,28.544)) + (3 * (1 - _.t0) * Square(_.t0) * Vector2(79.72001,10.328)) + (Pow(_.t0, 3) * Vector2(59.691,9.121))", _stepEasingFunction);
                result.InsertKeyFrame(0.700000048F, new Vector2(59.6910019F, 9.12100029F), _stepEasingFunction);
                return result;
            }

            Instantiator(Compositor compositor)
            {
                _c = compositor;
                _reusableExpressionAnimation = compositor.CreateExpressionAnimation();
            }

            public static Visual InstantiateComposition(Compositor compositor)
                => new Instantiator(compositor).Root();
        }
    }
}
