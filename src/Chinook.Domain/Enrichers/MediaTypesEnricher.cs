using Chinook.Domain.ApiModels;
using Chinook.Domain.Helpers;

namespace Chinook.Domain.Enrichers;

public class MediaTypesEnricher : ListEnricher<List<MediaTypeApiModel>>
{
    private readonly MediaTypeEnricher _enricher;

    public MediaTypesEnricher(MediaTypeEnricher enricher)
    {
        _enricher = enricher;
    }

    public override async Task Process(object representations)
    {
        foreach (var mediatype in (IEnumerable<MediaTypeApiModel>)representations)
        {
            await _enricher.Process(mediatype as MediaTypeApiModel);
        }
    }
}