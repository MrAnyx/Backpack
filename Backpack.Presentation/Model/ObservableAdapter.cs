using CommunityToolkit.Mvvm.ComponentModel;
using Mapster;

namespace Backpack.Presentation.Model;

public abstract class ObservableAdapter<TEntity, TWrapper> : ObservableObject
    where TEntity : Domain.Model.Entity
    where TWrapper : ObservableObject
{
    private readonly TypeAdapterConfig _config;

    protected ObservableAdapter()
    {
        _config = new TypeAdapterConfig();
        ConfigureMapping(_config);
    }

    // Override this in derived class to define mapping config
    protected virtual void ConfigureMapping(TypeAdapterConfig config)
    {
        config.NewConfig<TEntity, TWrapper>();
        config.NewConfig<TWrapper, TEntity>();
    }

    public void FromEntity(TEntity entity) => entity.Adapt(this, _config);
    public TEntity ToEntity() => this.Adapt<TEntity>(_config);
}
