(ns day3.sentences
  (:require [clojure.string :refer [trim]]))

(defn sentence-split [text]
  (map trim (re-seq #"[^\.!\?:;]+[\.!\?:;]*" text)))

(sentence-split "This is a sentence. Is this?! A fragment")

(defn is-sentence? [text]
  (re-matches #"^.*[\.!\?:;]$" text))

(is-sentence? "This is a sentence.")

(is-sentence? "A sentence doesn't end with a comma,")

(defn sentence-join [x y]
  (if (is-sentence? x) y (str x " " y)))

(defn strings->sentences [strings]
  (filter is-sentence?
    (reductions sentence-join
      (mapcat sentence-split strings))))

(def fragments ["A" "sentence." "And another." "Last" "sentence."])

(reductions sentence-join fragments)

(filter is-sentence? (reductions sentence-join fragments))

(strings->sentences fragments)


