using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AutoMapper;
using Common.Lib.Mapping;

namespace Common.Lib.Mapping
{
    /// <summary>
    /// This class Wraps AutoMapper so if we deceide to move to another mapper we can change implementation.
    /// Potential could use another mapper library like valueinjector.
    /// </summary>
    public class CommonMapper : ICommonMapper
    {
        /// <summary>
        /// This method uses Dependency resolution/registration as the fundemental pattern.
        /// It discovers all profile types that are assignable from the CommonMappingProfile class.
        /// Given the assemblies to search.
        /// </summary>
        /// <returns></returns>
        public CommonMapper(IEnumerable<string> assemblyNames)
        {
            Mapper.Reset();
            Mapper.Initialize(x => GetConfigurationByAssemblyNames(Mapper.Configuration, assemblyNames));

            //Make sure all objects hare mapped.
            Mapper.AssertConfigurationIsValid();
        }

        /// <summary>
        /// This method uses Dependency resolution/registration as the fundemental pattern.
        /// It discovers all profile types that are assignable from the CommonMappingProfile class.
        /// Given the assemblies to search.
        /// </summary>
        /// <returns></returns>
        public CommonMapper(IEnumerable<CommonMapperProfileResolutionSettings> settings)
        {
            Mapper.Reset();
            Mapper.Initialize(x => GetConfigurationByAssemblyNameAndNamespace(Mapper.Configuration, settings));

            //Make sure all objects hare mapped.
            Mapper.AssertConfigurationIsValid();
        }

        public CommonTypeMap FindTypeMapFor(Type sourceType, Type destinationType)
        {
            var typeMap = Mapper.FindTypeMapFor(sourceType, destinationType);
            return new CommonTypeMap(new AutoMapper.TypeInfo(typeMap.SourceType), new AutoMapper.TypeInfo(typeMap.DestinationType), typeMap.ConfiguredMemberList);
        }

        public CommonTypeMap FindTypeMapFor<TSource, TDestination>()
        {
            var typeMap = Mapper.FindTypeMapFor<TSource, TDestination>();
            return new CommonTypeMap(new AutoMapper.TypeInfo(typeMap.SourceType), new AutoMapper.TypeInfo(typeMap.DestinationType), typeMap.ConfiguredMemberList);
        }

        public CommonTypeMap[] GetAllTypeMaps()
        {
            var typeMap = Mapper.GetAllTypeMaps();
            return typeMap.Select(map => new CommonTypeMap(new AutoMapper.TypeInfo(map.SourceType), new AutoMapper.TypeInfo(map.DestinationType), map.ConfiguredMemberList)).ToArray();
        }

        public TDestination Map<TSource, TDestination>(TSource source)
        {
            return Mapper.Map<TSource, TDestination>(source);
        }

        public object Map(object source, Type sourceType, Type destinationType)
        {
            return Mapper.Map(source, sourceType, destinationType);
        }

        /// <summary>
        /// Gets the configuration by assembly names.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="assemblyNames">The assembly names.</param>
        private void GetConfigurationByAssemblyNames(IConfiguration configuration, IEnumerable<string> assemblyNames)
        {
            foreach (var assemblyName in assemblyNames)
            {
                var profiles = Assembly.Load(assemblyName).GetTypes().Where(x => typeof(CommonMappingProfile).IsAssignableFrom(x));
                foreach (var profile in profiles)
                {
                    configuration.AddProfile(Activator.CreateInstance(profile) as CommonMappingProfile);
                }
            }
        }

        /// <summary>
        /// Gets the configuration by assembly name and namespace.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="settings">The settings.</param>
        private void GetConfigurationByAssemblyNameAndNamespace(IConfiguration configuration, IEnumerable<CommonMapperProfileResolutionSettings> settings)
        {
            foreach (var setting in settings)
            {
                IEnumerable<Type> profiles;
                if (setting.Namespace != null && setting.Namespace.Any())
                    profiles = Assembly.Load(setting.AssemblyName).GetTypes().Where(x => typeof(CommonMappingProfile).IsAssignableFrom(x) && setting.Namespace.Contains(x.Namespace));
                else
                    profiles = Assembly.Load(setting.AssemblyName).GetTypes().Where(x => typeof(CommonMappingProfile).IsAssignableFrom(x));

                foreach (var profile in profiles)
                {
                    configuration.AddProfile(Activator.CreateInstance(profile) as CommonMappingProfile);
                }
            }
        }
    }
}
