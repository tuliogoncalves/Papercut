
import { Component } from '@angular/core';
import { BreakpointObserver, Breakpoints, BreakpointState } from '@angular/cdk/layout';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { NGXLogger } from 'ngx-logger';

import { MessageRepositoryService, MessageItem } from './message-repository.service';

interface MessageItemExtended extends MessageItem {
  isSelected?: boolean;
}

@Component({
  selector: 'papercut-app',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'Papercut';

  isHandset$: Observable<boolean> = this.breakpointObserver.observe(Breakpoints.Handset)
  .pipe(
    map(result => result.matches)
  );

  constructor(
    private breakpointObserver: BreakpointObserver,
    private messageRepository: MessageRepositoryService,
    private logger: NGXLogger)  {
      this.messageRepository.list().subscribe(m => {
        this.messages = m.messages;
      });

    }

    messages: MessageItemExtended[];

    public get message(): MessageItemExtended {
      return this.getSelected()[0];
    }

    public set message(message: MessageItemExtended) {
      this.clearSelected();
      message.isSelected = true;
    }

    public getSelected(): MessageItemExtended[] {
      if (!this.messages) {
        return new Array<MessageItemExtended>();
      }

      return this.messages.filter(message => message.isSelected);
    }

    public clearSelected() {
      this.getSelected().forEach(m => {
        m.isSelected = false;
      });
    }

    public selectMessage(selectedMessage: MessageItemExtended) {
      this.message = selectedMessage;

      this.logger.info('Selected Message', selectedMessage);
    }
}
