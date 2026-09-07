using System.Diagnostics.CodeAnalysis;
using ReactiveUI;

namespace MiraiSpace.Presentation.Foundation;

[SuppressMessage("Major Code Smell", "S2094:Classes should not be empty",
    Justification = "Application-owned INPC base; initialization and activation are deliberately composed separately.")]
public abstract class ReactiveModel : ReactiveObject;
