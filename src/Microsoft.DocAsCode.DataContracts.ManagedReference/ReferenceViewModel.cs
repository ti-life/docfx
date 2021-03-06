// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.DocAsCode.DataContracts.ManagedReference
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;

    using Newtonsoft.Json;
    using YamlDotNet.Serialization;

    using Microsoft.DocAsCode.DataContracts.Common;
    using Microsoft.DocAsCode.YamlSerialization;

    [Serializable]
    public class ReferenceViewModel
    {
        [YamlMember(Alias = Constants.PropertyName.Uid)]
        [JsonProperty(Constants.PropertyName.Uid)]
        public string Uid { get; set; }

        [YamlMember(Alias = "parent")]
        [JsonProperty("parent")]
        public string Parent { get; set; }

        [YamlMember(Alias = "definition")]
        [JsonProperty("definition")]
        public string Definition { get; set; }

        [JsonProperty("isExternal")]
        [YamlMember(Alias = "isExternal")]
        public bool? IsExternal { get; set; }

        [YamlMember(Alias = Constants.PropertyName.Href)]
        [JsonProperty(Constants.PropertyName.Href)]
        public string Href { get; set; }

        [YamlMember(Alias = "name")]
        [JsonProperty("name")]
        public string Name { get; set; }

        [ExtensibleMember("name.")]
        [JsonIgnore]
        public SortedList<string, string> NameInDevLangs { get; } = new SortedList<string, string>();

        [YamlIgnore]
        [JsonIgnore]
        public string NameForCSharp
        {
            get
            {
                string result;
                NameInDevLangs.TryGetValue("csharp", out result);
                return result;
            }
            set { NameInDevLangs["csharp"] = value; }
        }

        [YamlIgnore]
        [JsonIgnore]
        public string NameForVB
        {
            get
            {
                string result;
                NameInDevLangs.TryGetValue("vb", out result);
                return result;
            }
            set { NameInDevLangs["vb"] = value; }
        }

        [YamlMember(Alias = "fullName")]
        [JsonProperty("fullName")]
        public string FullName { get; set; }

        [ExtensibleMember("fullname.")]
        [JsonIgnore]
        public SortedList<string, string> FullNameInDevLangs { get; } = new SortedList<string, string>();

        [YamlIgnore]
        [JsonIgnore]
        public string FullNameForCSharp
        {
            get
            {
                string result;
                FullNameInDevLangs.TryGetValue("csharp", out result);
                return result;
            }
            set { FullNameInDevLangs["csharp"] = value; }
        }

        [YamlIgnore]
        [JsonIgnore]
        public string FullNameForVB
        {
            get
            {
                string result;
                FullNameInDevLangs.TryGetValue("vb", out result);
                return result;
            }
            set { FullNameInDevLangs["vb"] = value; }
        }

        [ExtensibleMember("spec.")]
        [JsonIgnore]
        public SortedList<string, List<SpecViewModel>> Specs { get; } = new SortedList<string, List<SpecViewModel>>();

        [YamlIgnore]
        [JsonIgnore]
        public List<SpecViewModel> SpecForCSharp
        {
            get
            {
                List<SpecViewModel> result;
                Specs.TryGetValue("csharp", out result);
                return result;
            }
            set { Specs["csharp"] = value; }
        }

        [YamlIgnore]
        [JsonIgnore]
        public List<SpecViewModel> SpecForVB
        {
            get
            {
                List<SpecViewModel> result;
                Specs.TryGetValue("vb", out result);
                return result;
            }
            set { Specs["vb"] = value; }
        }

        [YamlMember(Alias = "type")]
        [JsonProperty("type")]
        public MemberType? Type { get; set; }

        [YamlMember(Alias = "summary")]
        [JsonProperty("summary")]
        public string Summary { get; set; }

        [YamlMember(Alias = "syntax")]
        [JsonProperty("syntax")]
        public SyntaxDetailViewModel Syntax { get; set; }

        [YamlMember(Alias = "platform")]
        [JsonProperty("platform")]
        public List<string> Platform { get; set; }

        [ExtensibleMember]
        [JsonIgnore]
        public Dictionary<string, object> Additional { get; } = new Dictionary<string, object>();

        [EditorBrowsable(EditorBrowsableState.Never)]
        [YamlIgnore]
        [JsonExtensionData(ReadData = false, WriteData = true)]
        public Dictionary<string, object> AdditionalJson
        {
            get
            {
                var dict = new Dictionary<string, object>();
                foreach (var item in NameInDevLangs)
                {
                    dict["name." + item.Key] = item.Value;
                }
                foreach (var item in FullNameInDevLangs)
                {
                    dict["fullname." + item.Key] = item.Value;
                }
                foreach (var item in Specs)
                {
                    dict["spec." + item.Key] = item.Value;
                }
                foreach (var item in Additional)
                {
                    dict[item.Key] = item.Value;
                }
                return dict;
            }
            set { }
        }
    }
}
