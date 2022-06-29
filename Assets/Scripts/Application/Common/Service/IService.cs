using System.Threading.Tasks;

public enum ServiceType {
    Rule
}

public interface ServiceStatePresenter {
    void ShowServiceState(string key);
}

public interface IService {
    ServiceType type { get; }
    Task<bool> Initialize(ServiceStatePresenter presenter);
}