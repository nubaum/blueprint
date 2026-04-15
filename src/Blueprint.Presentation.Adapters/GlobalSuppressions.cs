// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage(
    "Design",
    "BEV001:Avoid subscribing to events with '+='",
    Justification = "This is the only way it can be done here. And this is the core so it is acceptable.",
    Scope = "member",
    Target = "~M:Blueprint.Presentation.Adapters.CommandManagerHelper.Subscribe(System.EventHandler)")]
