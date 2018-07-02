import { TestBed, inject } from '@angular/core/testing';

import { MessageRepositoryService } from './message-repository.service';

describe('MessageRepositoryService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [MessageRepositoryService]
    });
  });

  it('should be created', inject([MessageRepositoryService], (service: MessageRepositoryService) => {
    expect(service).toBeTruthy();
  }));
});
