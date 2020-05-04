using HateoasNet.Abstractions;
using HateoasNet.Resources;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;

namespace HateoasNet.Framework.Serialization
{
  /// <inheritdoc cref="IHateoasSerializer"/>
  public class HateoasSerializer : IHateoasSerializer
  {
    /// <summary>
    ///   Constructor accepting an optional <see cref="JsonSerializerSettings"/> as <paramref name="settings" />.
    /// </summary>
    /// <param name="settings">Optional settings for custom output serialization.</param>
    public HateoasSerializer(JsonSerializerSettings settings = null)
    {
      _settings = settings ?? new JsonSerializerSettings
      {
        NullValueHandling = NullValueHandling.Ignore,
        ContractResolver = new CamelCasePropertyNamesContractResolver()
      };
    }

    private readonly JsonSerializerSettings _settings;

    public string SerializeResource(Resource resource)
    {
      if (resource == null) throw new ArgumentNullException(nameof(resource));

      return JsonConvert.SerializeObject(resource, resource.GetType(), _settings);
    }
  }
}
