/* 
* Copyright (C) Decartes Corp. All Rights Reserved
* Unauthorized copying of this file, via any medium is strictly prohibited
* Proprietary and confidential
* 
* @author You sungHyun <1seconds@biscuitlabs.io>
* @created 2022/06/29
* @desc IService Template
*/
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