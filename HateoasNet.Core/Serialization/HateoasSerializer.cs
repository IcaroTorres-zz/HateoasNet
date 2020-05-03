using HateoasNet.Abstractions;
using HateoasNet.Resources;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HateoasNet.Core.Serialization
{
  /// <inheritdoc cref="IHateoasSerializer" />
  public class HateoasSerializer : IHateoasSerializer
  {
    /// <summary>
    ///   Constructor accepting an optional <see cref="JsonSerializerOptions"/> as <paramref name="settings" />.
    /// </summary>
    /// <param name="settings">Optional settings for custom output serialization.</param>
    public HateoasSerializer(JsonSerializerOptions settings = null)
    {
      Settings = settings ?? new JsonSerializerOptions
      {
        IgnoreNullValues = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
      };
      Settings.Converters.Add(new GuidConverter());
      Settings.Converters.Add(new DateTimeConverter());
    }
    internal readonly JsonSerializerOptions Settings;

    public string SerializeResource(Resource resource)
    {
      if (resource == null) throw new ArgumentNullException(nameof(resource));

      return JsonSerializer.Serialize(resource, resource.GetType(), Settings);
    }

    /// <summary>
    ///   Adds a custom <see cref="JsonConverter{T}" /> to the configured collection of converters.
    /// </summary>
    /// <returns>Current <see cref="HateoasSerializer"/> instance.</returns>
    public HateoasSerializer AddConverter<T>(JsonConverter<T> converter)
    {
      if (converter == null) throw new ArgumentNullException(nameof(converter));

      Settings.Converters.Add(converter);
      return this;
    }

    /// <summary>
    ///   Clear the configured collection of JsonConverters.
    /// </summary>
    /// <returns>Current <see cref="HateoasSerializer"/> instance.</returns>
    public HateoasSerializer ResetConverters()
    {
      Settings.Converters.Clear();
      return this;
    }
  }
}
