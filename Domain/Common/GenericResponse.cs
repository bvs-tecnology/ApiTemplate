﻿using Domain.Common.ViewModel;

namespace Domain.Common;

public class GenericResponse<T>(T? result = null) : BaseResponse<T>(result) where T : BaseViewModel
{ }