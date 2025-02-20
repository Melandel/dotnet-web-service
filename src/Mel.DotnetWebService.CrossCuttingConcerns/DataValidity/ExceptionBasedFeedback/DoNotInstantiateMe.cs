namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ExceptionBasedFeedback;

public abstract class DoNotInstantiateMe { }
// ☝️ The class name indicates to the developer that it should not be instantiated
// Justification: Sometimes we need to help C# resolve certain method signatures in a controlled way.
//   The purpose of this class is to prevent the possibility of having the last argument explicitly provided match the same type of the type argument-with-a-default-value
//
// For instance,
//   Given the instruction
//     ObjectConstructionException.CreateFrom("my validation rule", "value of the 1st method parameter", "value of the 2nd method parameter")
//
//   🚫 If we don't use the class DoNotInstantiateMe, here's what happens:
//     Signature #1: ObjectConstructionException.CreateFrom(string validationRule,                                                             string callerMemberName = "")
//     Signature #2: ObjectConstructionException.CreateFrom(string validationRule, object singleParamValue,                                    string callerMemberName = "") --> C# prioritizes { "singleParamValue": "value of the 1st method parameter", "callerMemberName": "value of the 2nd method parameter" }
//     Signature #3: ObjectConstructionException.CreateFrom(string validationRule, object paramValue1, object paramValue2,                     string callerMemberName = "") --> But we wanted  { "paramValue1":      "value of the 1st method parameter", "paramValue2":      "value of the 2nd method parameter" }
//     Signature #4: ObjectConstructionException.CreateFrom(string validationRule, object paramValue1, object paramValue2, object paramValue3, string callerMemberName = "")
//
//   ✅ If we do use the class DoNotInstantiateMe, here's what happens:
//     Signature #1: ObjectionConstructionException.SomeMethodName(string validationRule,                                                             DoNotInstantiateMe? signatureResolutionMarker = null, string callerMemberName = "")
//     Signature #2: ObjectionConstructionException.SomeMethodName(string validationRule, object singleParamValue,                                    DoNotInstantiateMe? signatureResolutionMarker = null, string callerMemberName = "")
//     Signature #3: ObjectionConstructionException.SomeMethodName(string validationRule, object paramValue1, object paramValue2,                     DoNotInstantiateMe? signatureResolutionMarker = null, string callerMemberName = "") --> C# prioritizes this signature correctly, because the value
//     Signature #4: ObjectionConstructionException.SomeMethodName(string validationRule, object paramValue1, object paramValue2, object paramValue3, DoNotInstantiateMe? signatureResolutionMarker = null, string callerMemberName = "") "value of the 2nd method parameter" does NOT match the type DoNotInstantiateMe
