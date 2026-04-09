// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage(
    "Minor Code Smell",
    "S2325:Methods and properties that don't access instance data should be static",
    Justification = "This is a partial class so it needs to remain non static",
    Scope = "member",
    Target = "~M:Blueprint.App.OnDispatcherUnhandledException(System.Object,System.Windows.Threading.DispatcherUnhandledExceptionEventArgs)")]
[assembly: SuppressMessage(
    "Major Code Smell",
    "S1200:Classes should not be coupled to too many other classes",
    Justification = "Temporary solution",
    Scope = "type",
    Target = "~T:Blueprint.ServiceCollectionExtension")]
[assembly: SuppressMessage(
    "Design",
    "BEV001:Avoid subscribing to events with '+='",
    Justification = "This is the only exception",
    Scope = "member",
    Target = "~E:Blueprint.Services.WindowsCommandManager.RequerySuggested")]
