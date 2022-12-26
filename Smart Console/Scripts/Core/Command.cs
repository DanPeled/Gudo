using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Object = UnityEngine.Object;

namespace SmartConsole
{
    public class Command
    {
        public static readonly List<Command> List = new List<Command>();

        public readonly Object Target;
        public readonly MethodInfo MethodInfo;
        
        public Command(Object target, MethodInfo methodInfo)
        {
            Target = target;
            MethodInfo = methodInfo;
        }

        public void Use()
        {
            MethodInfo.Invoke(Target, null);
        }
        
        public void Use(string[] parametersParts)
        {
            var parametersInfos = MethodInfo.GetParameters();
            var parameters = new object[parametersInfos.Length];

            for (int i = 0; i < parametersInfos.Length; i++)
            {
                if (parametersInfos[i].ParameterType == typeof(int))
                {
                    try
                    {
                        parameters[i] = 
                            CastParameter<int>(parametersInfos[i], parametersParts.ElementAtOrDefault(i), TryParseInt);
                    }
                    catch (Exception e)
                    {
                        Debug.LogError(e);
                    }
                }
                else if (parametersInfos[i].ParameterType == typeof(bool))
                {
                    try
                    {
                        parameters[i] = 
                            CastParameter<bool>(parametersInfos[i], parametersParts.ElementAtOrDefault(i), TryParseBool);
                    }
                    catch (Exception e)
                    {
                        Debug.LogError(e);
                    }
                }
                else if (parametersInfos[i].ParameterType == typeof(float))
                {
                    try
                    {
                        parameters[i] = 
                            CastParameter<float>(parametersInfos[i], parametersParts.ElementAtOrDefault(i), TryParseFloat);
                    }
                    catch (Exception e)
                    {
                        Debug.LogError(e);
                    }
                }
                else if (parametersInfos[i].ParameterType == typeof(string))
                {
                    parameters[i] = parametersParts.ElementAtOrDefault(i);
                }
                else
                {
                    throw new NotSupportedException();
                }
            }

            MethodInfo.Invoke(Target, parameters);
        }

        private int TryParseInt(string toParse)
        {
            if (int.TryParse(toParse, out int n))
            {
                return n;
            }
            
            throw new InvalidCastException();
        }
        
        private bool TryParseBool(string toParse)
        {
            if (bool.TryParse(toParse, out bool state))
            {
                return state;
            }
            
            throw new InvalidCastException();
        }
        
        private float TryParseFloat(string toParse)
        {
            toParse = toParse.Replace('.', ',');
            
            if (float.TryParse(toParse, out float n))
            {
                return n;
            }
            
            throw new InvalidCastException();
        }
        
        private T CastParameter<T>(ParameterInfo parameterInfo, string parametersPart, Func<string, T> parse)
        {
            if (parameterInfo.ParameterType != typeof(T))
            {
                throw new NotSupportedException();
            }
            
            if (string.IsNullOrEmpty(parametersPart) && parameterInfo.HasDefaultValue)
            {
                return (T)parameterInfo.DefaultValue;
            }

            var parsedValue = parse(parametersPart);
            
            return parsedValue;
        }
    }
}
