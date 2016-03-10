import Darwin

// Utility functions and Thread class

func bridgeRetained<T : AnyObject>(obj : T) -> UnsafeMutablePointer<Void> {
    return UnsafeMutablePointer(Unmanaged.passRetained(obj).toOpaque())
}

func bridgeTransfer<T : AnyObject>(ptr : UnsafeMutablePointer<Void>) -> T {
    return Unmanaged<T>.fromOpaque(COpaquePointer(ptr)).takeRetainedValue()
}

final class Thread {
    private var thread = pthread_t(nil)
    private let function: () -> ()
    
    init(function: () -> ()) {
        self.function = function
        
        let context = bridgeRetained(self)
        
        pthread_create(&thread, nil, { arg in
            let mySelf: Thread = bridgeTransfer(arg)
            mySelf.function()
            return nil
        }, context)
    }
    
    func join() {
        pthread_join(thread, nil)
    }
}

// Reordering example

var x = 0
var y = 0

let t1 = Thread {
    while true {
        x += 1
        y += 1
    }
}

let t2 = Thread {
    while true {
        if x < y {
            print("Oh, dear!")
        }
    }
}

t1.join()
t2.join()