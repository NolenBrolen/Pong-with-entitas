//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class InputEntity {

    static readonly ChangeColor changeColorComponent = new ChangeColor();

    public bool isChangeColor {
        get { return HasComponent(InputComponentsLookup.ChangeColor); }
        set {
            if (value != isChangeColor) {
                if (value) {
                    AddComponent(InputComponentsLookup.ChangeColor, changeColorComponent);
                } else {
                    RemoveComponent(InputComponentsLookup.ChangeColor);
                }
            }
        }
    }
}

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentMatcherGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public sealed partial class InputMatcher {

    static Entitas.IMatcher<InputEntity> _matcherChangeColor;

    public static Entitas.IMatcher<InputEntity> ChangeColor {
        get {
            if (_matcherChangeColor == null) {
                var matcher = (Entitas.Matcher<InputEntity>)Entitas.Matcher<InputEntity>.AllOf(InputComponentsLookup.ChangeColor);
                matcher.componentNames = InputComponentsLookup.componentNames;
                _matcherChangeColor = matcher;
            }

            return _matcherChangeColor;
        }
    }
}
