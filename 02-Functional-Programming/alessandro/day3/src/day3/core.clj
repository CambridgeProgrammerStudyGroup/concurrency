(ns day3.core
  (:gen-class)
  (:require [clojure.edn        :as    edn]
            [day3.sentences   :refer [strings->sentences]]
            [compojure.core     :refer :all]
            [compojure.handler  :refer [site]]
            [ring.util.response :refer [response charset]]
            [ring.adapter.jetty :refer [run-jetty]]
            [clj-http.client    :as client]))

(def snippets (repeatedly promise))

(defn accept-snippet [n text] (deliver (nth snippets n) text))

;(future
;  (doseq [snippet (map deref snippets)]
;    (println snippet)))


(def translator "http://localhost:3001/translate")

(defn translate [text] (future
    (:body (client/post translator {:body text}))))

(def translations
  (delay
    (map translate
         (strings->sentences (map deref snippets)))))

(defn get-translation [n]
  (deref (nth (deref translations) n)))

(defroutes app-routes
  (PUT "/snippet/:n" [n :as {:keys [body]}]
    (accept-snippet (edn/read-string n) (slurp body))
    (response "OK"))
  (GET "/translation/:n" [n]
    (response (get-translation (edn/read-string n)))))

(defn wrap-charset [handler]
  (fn [req] (charset (handler req) "UTF-8")))

(defn -main [& args]
  (run-jetty (wrap-charset (site app-routes)) {:port 3000}))



