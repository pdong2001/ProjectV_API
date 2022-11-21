using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Utils.Helpers
{
    public static class TypeScriptHepler
    {
        public static Type[] GetTypesInNamespace(Assembly assembly, string nameSpace)
        {
            return
              assembly.GetTypes()
                      .Where(t => t.Namespace != null && t.Namespace.StartsWith(nameSpace, StringComparison.Ordinal))
                      .ToArray();
        }

        public static string GetTypeSriptTypeName(Type t)
        {
            var isNullableType = false;
            var hasDefaultValue = t.IsDefined(typeof(DefaultValueAttribute), true);
            string typeName = "any";
            var isArray = false;
            var isDictionary = false;
            var isGeneric = false;
            var genericTypeName = "";
            if (Nullable.GetUnderlyingType(t) != null)
            {
                t = Nullable.GetUnderlyingType(t);
                isNullableType = true;
            }
            if (t.IsGenericType && t.Name.Contains(typeof(Task<>).Name))
            {
                t = t.GetGenericArguments()[0];
            }
            isGeneric = t.IsGenericType;
            while (t.IsGenericType)
            {
                isArray = t.GetInterface(nameof(IEnumerable)) != null;
                isDictionary = t.GetInterface(nameof(IDictionary)) != null;
                if (string.IsNullOrEmpty(genericTypeName))
                {
                    if (isDictionary)
                    {
                        var keyType = t.GetGenericArguments()[0];
                        genericTypeName = $"{{[index:{GetTypeSriptTypeName(keyType.IsEnum ? typeof(int) : keyType)}] : {{type}}}}";
                    }
                    else if (isArray)
                    {
                        genericTypeName = "{type}[]";
                    }
                    else
                    {
                        genericTypeName = t.Name.Replace("`1", string.Empty) + "<{type}>";
                    }
                }
                else
                {
                    if (isDictionary)
                    {
                        var keyType = t.GetGenericArguments()[0];
                        genericTypeName = genericTypeName.Replace("{type}", $"{{[index:{GetTypeSriptTypeName(keyType.IsEnum ? typeof(int) : keyType)}] : {{type}}}}");
                    }
                    else if (isArray)
                    {
                        genericTypeName = genericTypeName.Replace("{type}", "{type}[]");
                    }
                    else
                    {
                        genericTypeName = genericTypeName.Replace("{type}", t.Name.Replace("`1", string.Empty) + "<{type}>");
                    }
                }
                t = t.GetGenericArguments().Last();
                isArray = false;
                isDictionary = false;
            }
            if (t.IsClass && t.GetCustomAttribute<RequiredAttribute>() == null)
            {
                isNullableType = true;
            }

            if ((t.IsClass || t.IsInterface || t.IsAbstract) && (!t.Namespace.StartsWith(nameof(System)) || t.IsGenericType) || t.IsEnum)
            {
                if (t.IsGenericType)
                {
                    typeName = t.GetGenericArguments()[0].Name;
                }
                else
                {
                    typeName = t.Name;
                }
            }
            else if (t.Equals(typeof(string)) || t.Equals(typeof(Guid)))
            {
                typeName = "string";
            }
            else if (t.Name.Contains(typeof(int).Name) || t.Name.Contains(typeof(long).Name))
            {
                typeName = "number";
            }
            else if (t.Name.Contains(typeof(DateTime).Name) || t.Name.Contains(typeof(long).Name))
            {
                typeName = "Date";
            }
            else if (t.Name.Contains(typeof(bool).Name))
            {
                typeName = "boolean";
            }
            if (isArray)
            {
                typeName += "[]";
            }
            if (isGeneric)
            {
                typeName = genericTypeName.Replace("{type}", typeName);
            }
            if (isNullableType)
            {
                typeName += " | null";
            }
            return typeName;
        }


        public class ImportClass
        {
            public ImportClass(string name, string path)
            {
                Name = name;
                Path = path.Replace("\\", "/");
            }

            public string Name { get; set; }
            public string Path { get; set; }
            public override string ToString()
            {
                return $"import {{ {Name} }} from \"{Path}\";\n";
            }
        }

        public static void CreateTypeScriptDto(Assembly assembly, string baseNameSpace, string baseSavePath)
        {
            var dtoClasses = TypeScriptHepler.GetTypesInNamespace(assembly, baseNameSpace).Where(t => !t.IsEnum && !t.IsAbstract && t.GetCustomAttribute<NoTScriptAttribute>() == null);
            try
            {
                if (Directory.Exists(baseSavePath))
                {
                    Directory.Delete(baseSavePath, true);
                }
            }
            catch { }


            foreach (var item in dtoClasses)
            {
                var mappedEnumNames = new List<string>();
                var typeName = item.Name.Replace("`1", string.Empty);
                var folderToSave = GetSavePath(item, baseNameSpace);
                if (!folderToSave.Contains(baseSavePath))
                {
                    folderToSave = Path.Combine(baseSavePath, folderToSave);
                }
                var propertiesString = "";
                var genericTypes = item.GetGenericArguments().Select(t => t.Name).ToList();
                var properties = item.GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public)
                    .OrderBy(p => p.PropertyType.Name.Length)
                    .ThenBy(p => p.Name.Length)
                    .ToList();
                var importedClass = GetAllImportClass(item, baseNameSpace);
                foreach (var p in properties)
                {
                    var underlyingType = Nullable.GetUnderlyingType(p.PropertyType) ?? p.PropertyType;
                    if (underlyingType.IsEnum)
                    {
                        if (!mappedEnumNames.Contains(underlyingType.Name))
                        {
                            mappedEnumNames.Add(underlyingType.Name);
                            CreateEnum(underlyingType, baseSavePath);
                        }
                    }
                    object value = null;
                    string valueString = "";
                    var defaultAttr = p.GetCustomAttribute<DefaultValueAttribute>();
                    if (!p.PropertyType.IsGenericParameter && !p.PropertyType.Equals(typeof(DateTime)))
                    {

                        if (defaultAttr != null)
                        {
                            value = defaultAttr.Value.ToString();
                        }
                        else if (p.PropertyType.Equals(typeof(Guid)))
                        {
                            value = "\"\"";
                        }
                        else if (p.PropertyType.IsValueType)
                        {
                            value = Activator.CreateInstance(p.PropertyType);
                        }
                        if (p.PropertyType.Equals(typeof(string)))
                        {
                            valueString = $"\"{value}\"";
                        }
                        else if (p.PropertyType.GetInterface(nameof(IDictionary)) != null)
                        {
                            valueString = "{}";
                        }
                        else if (p.PropertyType.GetInterface(nameof(IEnumerable)) != null && !p.PropertyType.Equals(typeof(string)))
                        {
                            valueString = "[]";
                        }
                        else
                        {
                            valueString = value != null ? (p.PropertyType.IsEnum ? $"{p.PropertyType.Name}.{(value is string ? value : Enum.GetName(p.PropertyType, value))}" : value.ToString()) : "null";
                        }
                        if (!string.IsNullOrEmpty(valueString)) valueString = " = " + valueString;
                        if (p.PropertyType.Equals(typeof(bool))) valueString = valueString.ToLower();
                    }
                    propertiesString += $"\t{p.Name.ToCamelCase()}{(string.IsNullOrEmpty(valueString) ? "!" : "")}: {TypeScriptHepler.GetTypeSriptTypeName(p.PropertyType)}{valueString};\n";

                }
                string genericDefination = "";
                if (genericTypes.Count > 0)
                {
                    genericDefination = $"<{string.Join(",", genericTypes.ToArray())}>";
                }
                var template = $"{string.Concat(importedClass.Select(c => c.ToString()))}\nexport class {typeName}{genericDefination} {{\n{propertiesString}}}";
                var fileName = Path.Combine(folderToSave, $"{typeName.Replace("`1", string.Empty)}.model.ts");
                if (!Directory.Exists(folderToSave))
                {
                    Directory.CreateDirectory(folderToSave);
                }

                using (StreamWriter sw = new StreamWriter(fileName, true))
                {
                    sw.Write(template);
                }
            }
        }

        private static void CreateEnum(Type item, string basePath)
        {
            var folderToSave = Path.Combine(basePath, GetSavePath(item, "Enums"));
            var propertiesString = "";
            var enumValues = Enum.GetValues(item).Cast<object>().ToList().Distinct();
            foreach (var enumValue in enumValues)
            {
                propertiesString += $"\t{enumValue} = {enumValue.GetHashCode()},\n";
            }
            var template = $"export enum {item.Name} {{\n{propertiesString}}}";
            var fileName = Path.Combine(folderToSave, $"{item.Name}.enum.ts");
            if (!Directory.Exists(folderToSave))
            {
                Directory.CreateDirectory(folderToSave);
            }

            using (StreamWriter sw = new StreamWriter(fileName, false))
            {
                sw.Write(template);
            }
        }

        private static string GetSavePath(Type t, string baseNamespace)
        {
            t = Nullable.GetUnderlyingType(t) ?? t;
            if (t.GetInterface(nameof(IEnumerable)) != null)
            {
                t = t.GetGenericArguments()[0];
            }

            string folderToSave;
            if (t.IsEnum)
            {
                folderToSave = "Enums";
            }
            else
            {
                folderToSave = t.Namespace.Replace(baseNamespace, string.Empty).Trim('.').Replace(".", "\\");
            }

            return folderToSave;
        }

        private static List<ImportClass> GetAllImportClass(Type type, string baseNamespace)
        {
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(p => !p.PropertyType.Equals(type)).ToArray();
            List<ImportClass> imports = new List<ImportClass>(properties.Length);
            foreach (var pro in properties)
            {
                var p = pro.PropertyType;
                if (p.IsGenericType)
                {
                    var genericArgs = p.GetGenericArguments();
                    genericArgs.Where(a => !a.Namespace.StartsWith(nameof(System)) && !a.IsGenericParameter)
                        .ToList().ForEach(a =>
                        {
                            if (!imports.Any(i => i.Name == a.Name))
                            {
                                imports.Add(GetImportPath(type, a, baseNamespace));
                            }
                        });
                }
                else if (((p.IsClass && !p.IsGenericParameter && !p.Namespace.StartsWith(nameof(System))) || p.IsEnum) && !imports.Any(i => i.Name == p.Name))
                {
                    imports.Add(GetImportPath(type, p, baseNamespace));
                }
            }
            return imports;
        }

        private static ImportClass GetImportPath(Type owner, Type import, string baseNamespace)
        {
            var ownerNamespace = owner.Namespace.Replace(baseNamespace, string.Empty).Trim('.', ' ').Split('.').Where(i => !string.IsNullOrWhiteSpace(i)).ToList();
            var importNamespace = import.Namespace.Replace(baseNamespace, string.Empty).Trim('.', ' ').Split('.').Where(i => !string.IsNullOrWhiteSpace(i)).ToList();
            var pathToImport = "";

            for (int i = 0; i < ownerNamespace.Count; i++)
            {
                pathToImport += "../";
            }
            if (import.IsEnum)
            {
                if (!string.IsNullOrWhiteSpace(pathToImport))
                {
                    pathToImport = "../Enums";
                }
                else
                {
                    pathToImport = "./Enums";
                }

                pathToImport = Path.Combine(pathToImport, $"{import.Name}.enum");
            }
            else
            {
                if (string.IsNullOrEmpty(pathToImport))
                {
                    pathToImport = "./";
                }

                for (int i = 0; i < importNamespace.Count; i++)
                {
                    pathToImport += importNamespace[i] + "/";
                }
                pathToImport.TrimEnd('/');

                pathToImport = Path.Combine(pathToImport, $"{import.Name}.model");
            }
            var importClass = new ImportClass(import.Name, pathToImport);

            return importClass;
        }

        public static string ToCamelCase(this string str)
        {
            var words = str.Split(new[] { "_", " " }, StringSplitOptions.RemoveEmptyEntries);
            var leadWord = Regex.Replace(words[0], @"([A-Z])([A-Z]+|[a-z0-9]+)($|[A-Z]\w*)",
                m =>
                {
                    return m.Groups[1].Value.ToLower() + m.Groups[2].Value.ToLower() + m.Groups[3].Value;
                });
            var tailWords = words.Skip(1)
                .Select(word => char.ToUpper(word[0]) + word.Substring(1))
                .ToArray();
            return $"{leadWord}{string.Join(string.Empty, tailWords)}";
        }

        public static string ToUpperFirst(this string str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return str;
            }

            return str.Substring(0, 1).ToUpper() + str.Substring(1).ToLower();
        }

        [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false)]
        public class NoTScriptAttribute : Attribute { }
    }
}
