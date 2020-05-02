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
      _settings = settings ?? new JsonSerializerOptions
      {
        IgnoreNullValues = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
      };
      _settings.Converters.Add(new GuidConverter());
      _settings.Converters.Add(new DateTimeConverter());
    }
    private readonly JsonSerializerOptions _settings;

    public string SerializeResource<TResource>(TResource resource) where TResource : Resource
    {
      if (resource == null) throw new ArgumentNullException(nameof(resource));

      return JsonSerializer.Serialize(resource, typeof(TResource), _settings);
    }

    /// <summary>
    ///   Adds a custom <see cref="JsonConverters{T}" /> to the configured collection of converters.
    /// </summary>
    /// <returns>Current <see cref="HateoasSerializer"/> instance.</returns>
    public HateoasSerializer AddConverter<T>(JsonConverter<T> converter)
    {
      _settings.Converters.Add(converter);
      return this;
    }

    /// <summary>
    ///   Clear the configured collection of JsonConverters.
    /// </summary>
    /// <returns>Current <see cref="HateoasSerializer"/> instance.</returns>
    public HateoasSerializer ResetConverters()
    {
      _settings.Converters.Clear();
      return this;
    }
  }
}