interface IPerson {
    getFullName(): string;
}

class Person implements IPerson {

    private firstName: string;
    private lastName: string;

    setFirstName(value: string) {
        this.firstName = value;
    }

    setLastName(value: string) {
        this.lastName = value;
    }

    getFullName(lastNameFirst: boolean = false): string {
        if (lastNameFirst) {
            return this.lastName + ", " + this.firstName;
        }
        return this.firstName + ", " + this.lastName;
    }
}

class Chat {
    private chatBox: string;
    private manager: string;
    private client: string;
    private channel: string;
    private username: string;

    createChannel() { }

    print(infoMessage: string, isHtml: boolean) {
        const $msg = $("<div class=\"info\">");
        if (asHtml) {
            $msg.html(infoMessage);
        } else {
            $msg.text(infoMessage);
        }
        $chatWindow.append($msg);
    }
}